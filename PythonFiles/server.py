import math
import time
import zmq
import cv2
import mediapipe as mp
import json


context = zmq.Context()
socket = context.socket(zmq.PUB)
socket.bind("tcp://*:5555")

WIDTH, HEIGHT = 1280, 720



class Hand:
    def __init__(self, landmarks):
        x_sum, y_sum = 0, 0
        for lm in landmarks.landmark:
            x_sum += lm.x * WIDTH
            y_sum += lm.y * HEIGHT
        self.x = int(x_sum / 21)
        self.y = int(y_sum / 21)
        self.fist = self.is_fist(landmarks)

    # Check if fingers are folded to form a fist
    def is_fist(self, landmarks):
        fingers_folded = 0
        finger_tips = [8, 12, 16, 20]
        finger_pips = [6, 10, 14, 18]

        for tip, pip in zip(finger_tips, finger_pips):
            if landmarks.landmark[tip].y > landmarks.landmark[pip].y:
                fingers_folded += 1

        return fingers_folded >= 3  # Majority of fingers folded


class Head:
    def __init__(self, landmarks, frame_shape):
        # Use some key points to average head position (forehead, nose, chin)
        key_indices = [10, 152, 1]  # Forehead, chin, nose tip
        h, w, _ = frame_shape
        x_sum = y_sum = 0

        for idx in key_indices:
            x_sum += landmarks.landmark[idx].x * w
            y_sum += landmarks.landmark[idx].y * h

        self.x = int(x_sum / len(key_indices))
        self.y = int(y_sum / len(key_indices))
        self.mouth_open = self.is_mouth_open(landmarks)

    def is_mouth_open(self, landmarks):
        top_lip = landmarks.landmark[13]
        bottom_lip = landmarks.landmark[14]
        distance = math.hypot(top_lip.y - bottom_lip.y, top_lip.x - bottom_lip.x)
        return distance > 0.03


mp_hands = mp.solutions.hands
hands = mp_hands.Hands()
mp_face_mesh = mp.solutions.face_mesh
face_mesh = mp_face_mesh.FaceMesh(static_image_mode=False, max_num_faces=1)

cap = cv2.VideoCapture(0)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, WIDTH)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, HEIGHT)


while True:
    ret, frame = cap.read()
    if not ret:
        break

    frame = cv2.flip(frame, 1)
    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    hand_results = hands.process(rgb)
    face_result = face_mesh.process(rgb)

    _hands = []
    if hand_results.multi_hand_landmarks:
        for hand_landmarks in hand_results.multi_hand_landmarks:
            _hands.append(Hand(hand_landmarks))

    _head = None
    if face_result.multi_face_landmarks:
        for face_landmarks in face_result.multi_face_landmarks:
            _head = Head(face_landmarks, frame.shape)

    # Wait for next request from client
    time.sleep(0.001)

    # Prepare JSON object
    response_data = {
        "head": _head.__dict__ if _head else None,
        "hands": [hand.__dict__ for hand in _hands],
    }

    # Send the JSON string
    socket.send_string(json.dumps(response_data))
