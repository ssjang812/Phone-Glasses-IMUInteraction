# Phone-Glasses-IMUInteraction: Integrated Interaction Module for Smart Augmented Reality (AR)

---

## 1. Introduction

This repository presents an **Integrated Interaction Module** designed for seamless data reception and processing from the **Microsoft HoloLens 2** and **Xsens DOT IMU sensors** within a dedicated smartphone application environment.

The core objective of this module is to establish an efficient and intuitive UI/UX platform in an AR setting. This is achieved by enabling remote **input event reception** via a smartphone application and providing robust **3D object manipulation** functionalities based on the processed sensor and input data.

---

## 2. Development Environment

| Component | Version/Specification | Notes |
| :--- | :--- | :--- |
| **Unity Engine** | 2022.3.60f1 | **LTS version required.** |
| **Primary Target Device** | Microsoft HoloLens 2 | For AR rendering and object manipulation. |
| **Companion Device** | Smartphone (Android OS) | For remote input and IMU data reception. |
| **Sensor Integration** | Xsens DOT IMU Sensor | Real-time motion and orientation data source. |

---

## 3. Core Features

| Feature Category | Description |
| :--- | :--- |
| **HoloLens 2 Integration** | Module for receiving and processing input events transmitted remotely from the smartphone application to the HoloLens 2 environment. |
| **Object Manipulation** | Functionality for **3D Object Control and Manipulation** within the HoloLens 2 environment, driven by the received input events and sensor data. |
| **IMU Sensor Data** | Real-time **Orientation and Motion Data Reception Module** from the Xsens DOT IMU sensor via Bluetooth. |

*Detailed usage instructions, module integration guides, and API documentation will be updated progressively.*

---

## 4. Acknowledgement and Funding

This work was supported by Institute of Information & communications Technology Planning & Evaluation (IITP) grant funded by the Korea government (Ministry of Science and ICT, MSIT).

* **Grant No.:** 2019-0-01270
* **Project Title:** WISE AR UI/UX Platform Development for Smartglasses

---

## 5. Setup and Quick Start

This project requires a dual setup: a UWP build for the HoloLens 2 and an Android build for the smartphone interaction module. Follow these steps to prepare your development environment and deploy the application.

### 5.1 Prerequisites

Ensure you have access to the following hardware and software:

* Microsoft HoloLens 2
* Xsens DOT IMU Sensor(s)
* A Windows PC (Visual Studio 2022 recommended)
* A compatible Android Smartphone (for the companion app)

### 5.2 Development Environment Setup

Successful deployment requires specific Unity and platform configurations for both UWP and Android.

1.  **Unity Installation and Modules**:
    * Install **Unity Engine Version 2022.3.60f1** via the Unity Hub.
    * During installation, ensure the **Universal Windows Platform (UWP) Build Support** and **Android Build Support** modules are both included.
    
2.  **HoloLens 2 Development Setup (UWP)**:
    * Set up your development environment for Mixed Reality and the HoloLens 2 using the official Microsoft guide. This includes configuring Visual Studio and necessary SDKs.
    * **Guide:** [MRTK Tutorials - HoloLens 2 Development Setup](https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/)
    
3.  **Android Build Setup**:
    * Configure the Android SDK, NDK, and OpenJDK settings required for building Android applications within Unity.
    * **Guide:** [Unity Manual - Android SDK Setup](https://docs.unity3d.com/Manual/android-sdksetup.html)

### 5.3 Project Download and Initial Run

1.  **Download the Repository**:
    * Clone the repository using Git:
        ```bash
        git clone [Your Repository URL] 
        ```

2.  **Open in Unity**:
    * Open the downloaded project folder using Unity Hub with the installed **2022.3.60f1** version.

3.  **Build the Main Scene**:
    * Locate the primary scene (e.g., `Main.unity`) in the project.
    * Build and deploy the scene onto your target devices:
        * For the **HoloLens 2**, switch the platform to UWP and build.
        * For the **Smartphone**, switch the platform to Android and build (APK).

---

## 6. Additional Notes and Compatibility

### 6.1 Xsens DOT IMU Integration Details

The communication with the Xsens DOT sensors is implemented by packaging and utilizing the **Xsens DOT Native API** within the Unity environment. This allows for low-level, efficient sensor data access.

For researchers interested in native Android application development or requiring deeper sensor information, please refer to the official Xsens DOT documentation:

* **Xsens DOT Official Documentation:** [Xsens DOT Landing Page](https://base.movella.com/s/xsens-dot-landing-page?language=en_US)

### 6.2 WISE AR UI/UX Platform Compatibility

This module is designed to be compatible with and extend the core WISE AR UI/UX HoloLens 2 applications. When the respective projects are set up and running on the HoloLens 2, this module will function as a remote input and IMU data gateway, automatically establishing communication.

* **WISEUIUX Main Application:** [https://github.com/IkbeomJeon/WiseUI](https://github.com/IkbeomJeon/WiseUI)
* **GGSGEXT (Extension/Component):** [https://github.com/ssjang812/GGSGEXT](https://github.com/ssjang812/GGSGEXT)

Please follow the installation and deployment instructions provided in each corresponding repository. Further details on system integration will be updated soon.