import math
import time
import zmq
import cv2
import mediapipe as mp
import json

context = zmq.Context()
socket = context.socket(zmq.PUB)
socket.bind("tcp://*:5555")

# Full camera capture resolution
CAPTURE_WIDTH, CAPTURE_HEIGHT = 1920, 1080

# Reduced processing resolution
PROC_WIDTH, PROC_HEIGHT = 640, 360

class Hand:
    def __init__(self, landmarks, handedness, scale_x, scale_y):
        x_sum, y_sum = 0, 0
        for lm in landmarks.landmark:
            x_sum += lm.x * PROC_WIDTH * scale_x
            y_sum += lm.y * PROC_HEIGHT * scale_y
        self.x = int(x_sum / 21)
        self.y = int(y_sum / 21)
        self.fist = self.is_fist(landmarks)
        self.active = True
        self.hand_type = handedness.classification[0].label

    def is_fist(self, landmarks):
        fingers_folded = 0
        finger_tips = [8, 12, 16, 20]
        finger_pips = [6, 10, 14, 18]

        for tip, pip in zip(finger_tips, finger_pips):
            if landmarks.landmark[tip].y > landmarks.landmark[pip].y:
                fingers_folded += 1

        return fingers_folded >= 3

class Head:
    def __init__(self, landmarks, scale_x, scale_y):
        key_indices = [10, 152, 1]  # Forehead, chin, nose tip
        x_sum = y_sum = 0

        for idx in key_indices:
            x_sum += landmarks.landmark[idx].x * PROC_WIDTH * scale_x
            y_sum += landmarks.landmark[idx].y * PROC_HEIGHT * scale_y

        self.x = int(x_sum / len(key_indices))
        self.y = int(y_sum / len(key_indices))
        self.mouth_open = self.is_mouth_open(landmarks)
        self.active = True

    def is_mouth_open(self, landmarks):
        top_lip = landmarks.landmark[13]
        bottom_lip = landmarks.landmark[14]
        distance = math.hypot(top_lip.y - bottom_lip.y, top_lip.x - bottom_lip.x)
        return distance > 0.02

mp_hands = mp.solutions.hands
hands = mp_hands.Hands()
mp_face_mesh = mp.solutions.face_mesh
face_mesh = mp_face_mesh.FaceMesh(static_image_mode=False, max_num_faces=1)

cap = cv2.VideoCapture(0)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, CAPTURE_WIDTH)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, CAPTURE_HEIGHT)

# Scale factors to go from processing size to capture size
scale_x = CAPTURE_WIDTH / PROC_WIDTH
scale_y = CAPTURE_HEIGHT / PROC_HEIGHT

while True:
    ret, frame = cap.read()
    if not ret:
        break

    frame = cv2.flip(frame, 1)

    # Downscale frame for faster processing
    small_frame = cv2.resize(frame, (PROC_WIDTH, PROC_HEIGHT))
    rgb_small = cv2.cvtColor(small_frame, cv2.COLOR_BGR2RGB)

    # Process smaller frame
    hand_results = hands.process(rgb_small)
    face_result = face_mesh.process(rgb_small)

    _hands = []
    if hand_results.multi_hand_landmarks:
        for hand_landmarks, handedness in zip(hand_results.multi_hand_landmarks, hand_results.multi_handedness):
            _hands.append(Hand(hand_landmarks, handedness, scale_x, scale_y))

    _head = None
    if face_result.multi_face_landmarks:
        for face_landmarks in face_result.multi_face_landmarks:
            _head = Head(face_landmarks, scale_x, scale_y)

    time.sleep(0.001)

    response_data = {
        "head": _head.__dict__ if _head else None,
        "hands": [hand.__dict__ for hand in _hands],
    }

    socket.send_string(json.dumps(response_data))
