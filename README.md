# Hue-Shift
색을 바꿔 싸우는 3인칭 3D 액션 게임

## 개발 환경
- Unity 6000.3.18f1 (URP)
- Input System 1.19.0
- AI Navigation 2.0.13

## 프로젝트 열기 전: 외부 에셋 설치
이 저장소에는 유니티 에셋스토어 에셋이 포함되어 있지 않습니다. (재배포 불가 라이선스)
`Assets/99_Resources/` 폴더는 `.gitignore`로 제외되어 있어, 아래 에셋을 직접 설치해야 모델과 애니메이션이 표시됩니다.

| 용도 | 에셋 | 링크 |
|---|---|---|
| 플레이어 모델·애니메이션 | Battle Wizard Poly Art | https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/battle-wizard-poly-art-128097



### 설치 방법
1. 프로젝트를 Unity로 엽니다. (이 시점에는 모델이 비어 보이는 것이 정상입니다)
2. `Window > Package Manager > My Assets`에서 위 에셋을 Import 합니다.
3. 임포트된 폴더를 **Unity Project 창에서** `Assets/99_Resources/` 아래로 드래그해 옮깁니다.
   - 탐색기에서 옮기면 참조가 끊어집니다.
   - 옮기지 않으면 `.gitignore`에 걸리지 않아 Git에 수천 개의 파일이 변경으로 표시됩니다.

에셋 파일의 고유 ID(GUID)가 패키지에 포함되어 있어, 임포트만 하면 씬·프리팹·애니메이터의 참조는 자동으로 다시 연결됩니다.

### 에셋 원본에 적용한 설정 (직접 적용 필요)
아래 설정은 `99_Resources` 안에 저장되므로 저장소에 포함되지 않습니다.
게임 실행 전 설정이 맞게 되어있는지 확인해주세요.

| 대상 | 설정 | 이유 |
|---|---|---|
| 플레이어 FBX | 상체 공격용 Avatar Mask |
| 공격 애니메이션 클립 (StandAttack, MoveAttack) | Loop Time **해제** | 공격 1회 재생 |
| 이동 애니메이션 클립 | Loop Time **체크** | 이동 반복 재생 |
| 플레이어 머티리얼 | Emission **체크**, 색 검정 | 피격 시 흰색 번쩍임 (PlayerHitFlash) |
