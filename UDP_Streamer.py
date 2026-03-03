import cv2
import torch

# YOLOv5 modelini yükle
model = torch.hub.load('ultralytics/yolov5', 'yolov5s')

# Unity'den gelen UDP akışını aç
cap = cv2.VideoCapture('udp://127.0.0.1:5000', cv2.CAP_FFMPEG)

while True:
    ret, frame = cap.read()
    if not ret:
        continue

    results = model(frame)  # Nesne tespiti
    annotated_frame = results.render()[0]  # YOLOv5 çizimli görüntü

    cv2.imshow("YOLOv5 - Unity Camera", annotated_frame)
    if cv2.waitKey(1) == 27:  # ESC ile çıkış
        break

cap.release()
cv2.destroyAllWindows()
