# YOLOv5 Real-Time Object Detection in Unity Simulation
This project creates a real-time bridge between a Unity 3D environment and YOLOv5 for object detection. It focuses on streaming simulation data through an optimized pipeline to test computer vision models in controlled virtual environments.

![Image](https://github.com/user-attachments/assets/86743ca9-721c-419d-a93c-4c47f71cde8b)

## System Architecture
Data Source: A unity camera that moves orbitally around assets and captures various angles of assets to maximize detection diversity.

Streaming Engine: Integrated FFmpeg as a sub-process to pipe simulation image via UDP. This method was chosen over WebRTC to simplify the pipeline while streaming.

Inference: A Python-based server receives the UDP packets, decodes the stream using OpenCV, runs inference using pre-trained YOLOv5s model, and displays the direct stream with assets labeled.

## Engineering Challenges & Solutions
### 1. The Orientation Flip

Challenge: Initial streams appeared inverted due to differences in coordinate systems between Unity's render engine and OpenCV's frame buffers.

Solution: Instead of heavy post-processing in Python, I implemented a script-based camera transformation in Unity to flip the source frames, ensuring zero overhead on the inference side.

### 2. Stream Latency & Synchronization

Challenge: Observed a significant delay (latency) between the simulation and the YOLOv5 output.

Current Status: Identified that the bottleneck occurs during the raw data transfer between Unity’s Update loop and the FFmpeg process.

Analysis: While using UDP the synchronization between the high-frequency game loop and the video encoder still remains a technical issue.

### 3. Simulation-to-Real Domain Gap

Challenge: Since the model uses pre-trained weights (COCO dataset), some of the simulation assets were misidentified (e.g., a dog identified as a horse, or a car labeled as a truck).

Solution: To improve accuracy without retraining, I specifically selected high-fidelity 3D assets that closely match the visual features of the COCO dataset, achieving high confidence scores in standard lighting conditions.

## Project Structure
CameraCapture.cs: Manages the FFmpeg process and raw data piping.

CameraMovement.cs: Handles the 360-degree orbit movement of camera for multi-angle detection.

UDP_Streamer.py: The Python bridge that runs the YOLOv5 model.
