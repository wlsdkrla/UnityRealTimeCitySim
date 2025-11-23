# UrbanTrafficSim – Real-Time Traffic & Weather Visualization

UrbanTrafficSim은 FastAPI와 Unity를 활용해
실시간 도로 속도(Seoul Traffic API) 와
실시간 기온/날씨(기상청 API) 를 받아,
도시 전체의 교통 흐름, 차량 스폰, 날씨 연출을
인터랙티브 3D 시뮬레이션으로 시각화하는 프로젝트입니다.

Unity에서는 차량 이동 AI, 레이캐스트 기반 차간거리 제어,
정체 구간 자동 스폰, 날씨별 Skybox 변경, VFX 효과 등을 처리합니다.

📌 Features
🚗 Real-Time Traffic Simulation
<img width="925" height="346" alt="image" src="https://github.com/user-attachments/assets/1b10f617-11de-47b2-a1fb-9fb1cdf6fe45" />
<img width="867" height="458" alt="image" src="https://github.com/user-attachments/assets/f0ce9702-6ae2-40bf-978b-6d177a4c10ba" />
<img width="879" height="466" alt="image" src="https://github.com/user-attachments/assets/34bdfa16-076a-4161-942d-6f98fd115998" />

서울 열린데이터 포털 TrafficInfo API 연동

구간별 속도(link_id) 기반으로 차량 생성 빈도 자동 조절

정체 구간(저속)

원활 구간(고속)

Bus, Taxi, Car 등 랜덤 모델 스폰 지원

Raycast 기반 차간거리 제어(추돌 방지)

🌡️ Realtime Weather Visualization

기상청 SFCTM 데이터 연동

평균 기온/바람 속도 기반 Skybox 톤 조정

기온에 따라 자동 VFX 출력

더위 → Heatwave / Light Dust

추움 → Fog / Snow

비 → Rain VFX

🎥 Multiple Camera View Modes

Space 키로 카메라 전환


🔌 FastAPI Server

Python FastAPI 서버에서
교통 + 날씨 데이터를 통합 JSON으로 제공
