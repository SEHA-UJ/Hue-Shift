# Hue-Shift
색을 바꿔 싸우는 3인칭 3D 액션 게임

플레이어는 빨강·파랑·초록 세 가지 색을 바꿔가며 싸웁니다. 같은 색으로 공격해야 적에게 데미지가 들어가고,
다른 색의 공격에만 플레이어가 맞습니다.
공격할 때는 적의 색에 맞추고, 피할 때는 적 공격의 색에 맞춰야 합니다.

## 조작 방법
| 키 | 동작 |
|---|---|
| W A S D | 이동 |
| E | 색 변경 (빨 → 파 → 초) |
| Space | 점프 |
| 마우스 | 시점 회전 |
| 마우스 왼쪽 | 공격 (현재 색의 투사체 발사) |

## 색 규칙
| 상황 | 데미지 조건 |
|---|---|
| 플레이어 → 적 | 같은 색일 때만 |
| 적 → 플레이어 | 다른 색일 때만 |
| 보스 공격 색 | 보스 몸 색을 제외한 2색 중 랜덤 |

## 개발 환경
- Unity 6000.3.18f1 (URP)
- Input System 1.19.0
- AI Navigation 2.0.13

## 프로젝트 열기 전: 외부 에셋 설치
이 저장소에는 유니티 에셋스토어 에셋이 포함되어 있지 않습니다. (재배포 불가 라이선스)
`Assets/99_Resources/` 폴더는 `.gitignore`로 제외되어 있어, 아래 에셋을 직접 설치해야 모델과 애니메이션이 표시됩니다.

| 용도 | 에셋 | 링크 |
|---|---|---|
| 플레이어 모델·애니메이션 | Battle Wizard Poly Art | https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/battle-wizard-poly-art-128097 |
| 일반몹 - 데몬 (빨강) | [FreeTrail] Fantasy Monster 37 Game Ready Creatures | PixeliusVita | https://assetstore.unity.com/packages/3d/characters/creatures/freetrail-fantasy-monster-37-game-ready-creatures-pixeliusvita-382234 |
| 일반몹 - 해골 (파랑) | Mini Simple Characters | Skeleton | Free Demo | https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-simple-characters-skeleton-free-demo-262897 |

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

**플레이어**

| 대상 | 설정 | 이유 |
|---|---|---|
| 공격 애니메이션 클립 (StandAttack, MoveAttack) | Loop Time **해제** | 공격 1회 재생 |
| 이동 애니메이션 클립 | Loop Time **체크** | 이동 반복 재생 |
| 플레이어 머티리얼 | Emission **체크**, 색 검정 | 피격 시 흰색 번쩍임 (HitFlash) |

**일반몹 (데몬, 해골)**

| 대상 | 설정 | 이유 |
|---|---|---|
| 데몬·해골 머티리얼 | Emission **체크**, 색 검정 | 피격 시 흰색 번쩍임 (HitFlash) |

### Mixamo 애니메이션
일부 애니메이션은 [Mixamo](https://www.mixamo.com)에서 받았습니다. (Adobe 계정 필요, 무료)
Mixamo는 애니메이션별 고정 링크가 없으므로, 아래 이름으로 검색해 다운로드해 주세요.

| 사용처 | Animator 상태 | Mixamo 검색 이름 | In Place |
|---|---|---|---|
| 데몬/해골 | Idle | Breathing Idle | - |
| 데몬/해골 | Chase | Unarmed Run Forward | ✅ 체크 |
| 데몬/해골 | Attack | Standing Melee Attack | - |
| 데몬/해골 | Die | Sword And Shield Death | - |

**다운로드 옵션**

| 항목 | 값 |
|---|---|
| Format | FBX for Unity (.fbx) |
| Skin | Without Skin |
| Frames per Second | 30 |
| In Place | 이동 애니메이션만 체크 (이동은 NavMeshAgent가 하므로 제자리 재생) |

**저장 위치·이름**: `Assets/99_Resources/Mixamo/[몬스터명]/` 아래에 **표의 Animator 상태 이름과 같은 파일명**으로 저장합니다. (예: `Demon/Chase.fbx`)

**임포트 후 설정 (필수)**

| 대상 | 위치 (Inspector) | 설정 | 이유 |
|---|---|---|---|
| 몬스터 모델 FBX | Rig > Animation Type | **Humanoid** | Mixamo 애니메이션을 다른 모델에 입히려면(리타게팅) 양쪽 모두 Humanoid여야 함 |
| Mixamo 애니메이션 FBX 전부 | Rig > Animation Type | **Humanoid** (기본값 Generic에서 변경) | 위와 같음 |
| Mixamo 애니메이션 FBX 전부 | Rig > Avatar Definition | Create From This Model | |
| Idle, Chase 클립 | Animation > Loop Time | **체크** | 반복 재생 |
| Attack, Die 클립 | Animation > Loop Time | **해제** | 1회 재생 |

> 새로 다운로드한 Mixamo 파일은 파일 ID가 달라서, Animator의 각 상태(Idle, Chase, Attack, Die)에
> 클립을 **직접 다시 연결**해야 합니다. 상태 이름과 파일 이름을 같게 저장해 두면 찾기 쉽습니다.