import cv2
import mediapipe as mp
import pygame
import sys
import random
import math
import numpy as np

# Pygame setup
pygame.init()
WIDTH, HEIGHT = 800, 600
screen = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption("Hand Physics Game")
clock = pygame.time.Clock()

# Mediapipe hands
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(max_num_hands=2)
mp_drawing = mp.solutions.drawing_utils

# Webcam
cap = cv2.VideoCapture(0)


class Hand:
    def __init__(self, landmarks):
        self.landmarks = landmarks
        x_sum, y_sum = 0, 0
        for lm in landmarks.landmark:
            x_sum += lm.x * WIDTH
            y_sum += lm.y * HEIGHT
        self.x = int(x_sum / 21)
        self.y = int(y_sum / 21)

    def get_position(self):
        return (self.x, self.y)

    def draw(self, surface):
        if self.is_fist():
            pygame.draw.circle(surface, (0, 0, 0), (self.x, self.y), 50)
        else:
            pygame.draw.circle(surface, (255, 255, 255), (self.x, self.y), 50)

    # Check if fingers are folded to form a fist
    def is_fist(self):
        fingers_folded = 0
        finger_tips = [8, 12, 16, 20]
        finger_pips = [6, 10, 14, 18]

        for tip, pip in zip(finger_tips, finger_pips):
            if self.landmarks.landmark[tip].y > self.landmarks.landmark[pip].y:
                fingers_folded += 1

        return fingers_folded >= 3  # Majority of fingers folded


# Game object
class Ball:
    def __init__(self, x, y, radius=20):
        self.x = x
        self.y = y
        self.radius = radius
        self.vy = 0
        self.held_by = None

    def update(self):
        if self.held_by is None:
            self.vy += 0.5  # gravity
            self.y += self.vy
            if self.y > HEIGHT - self.radius:
                self.y = HEIGHT - self.radius
                self.vy *= -0.7

    def draw(self, surface):
        pygame.draw.circle(
            surface, (255, 0, 0), (int(self.x), int(self.y)), self.radius
        )


balls = [Ball(random.randint(100, 700), random.randint(100, 300)) for _ in range(6)]


# Main loop
while True:
    ret, frame = cap.read()
    if not ret:
        break

    frame = cv2.flip(frame, 1)
    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb)

    # Draw webcam as background
    resized = cv2.resize(rgb, (WIDTH, HEIGHT))
    pygame_frame = pygame.surfarray.make_surface(np.rot90(resized))
    screen.blit(pygame_frame, (0, 0))

    hand_positions = []
    _hands = []

    # Get average landmark positions for each hand
    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            _hands.append(Hand(hand_landmarks))

    for hand in _hands:
        hand_positions.append(hand.get_position())
        hand.draw(screen)

    # Ball logic
    for ball in balls:
        closest_hand = None
        min_dist = float("inf")

        for hand in _hands:
            dist = math.hypot(ball.x - hand.x, ball.y - hand.y)
            if dist < ball.radius + 40 and dist < min_dist and hand.is_fist():
                min_dist = dist
                closest_hand = hand

        if closest_hand is not None:
            ball.held_by = closest_hand
            ball.x = closest_hand.x
            ball.y = closest_hand.y
            ball.vy = 0
        else:
            ball.held_by = None

        ball.update()
        ball.draw(screen)

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            cap.release()
            pygame.quit()
            sys.exit()

    pygame.display.flip()
    clock.tick(30)
