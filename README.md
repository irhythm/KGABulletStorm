## KGABulletStorm

![Gameplay GIF](docs/gameplay.gif)

[![Unity](https://img.shields.io/badge/Unity-2022.3.61f1-black?logo=unity)](#)
[![TextMeshPro](https://img.shields.io/badge/TMP-3.0.7-0aa?logo=unity)](#)
[![Timeline](https://img.shields.io/badge/Timeline-1.7.7-0aa?logo=unity)](#)
[![VisualScripting](https://img.shields.io/badge/Visual%20Scripting-1.9.4-0aa?logo=unity)](#)
[![ToonShader](https://img.shields.io/badge/Toon%20Shader-0.12.0--preview-0aa?logo=unity)](#)

High-throughput bullet-storm style Unity project, structured around **event-driven game state** and **reuse-first runtime behavior** (avoid hot-path allocations and object churn).

### Quick start

- **Unity**: `2022.3.61f1` (from `ProjectSettings/ProjectVersion.txt`)
- **Open**: the project folder in Unity Hub → Open → Play in Editor

### Key Optimization Performance

> If no profiler logs exist in-repo, the table uses **repo-derived** evidence (code-path/count based). Rows explicitly marked where a “Before” value is a baseline assumption rather than measured.

| Measurement Metric | Before | After | Result (%) |
|---|---:|---:|---:|
| Bullet end-of-life uses `Destroy()` (object churn) | 1 call per expiry (legacy code path present but commented) | 0 calls per expiry (`SetActive(false)` in `Assets/Scripts/Attack/Bullet.cs`) | 100 |
| Spawn-time `Instantiate()` in gameplay scripts | 1 per spawn (baseline Unity pattern; not measured) | 0 occurrences in `Assets/Scripts/**/*.cs` (repo-derived) | 100 |

### Architecture (repo-observed, modular hub-and-subscribers)

The project’s runtime wiring centers on a **single state hub** (`GameManager`) that emits events. Systems subscribe/unsubscribe explicitly to prevent lifecycle leaks, and gameplay execution checks `IsPlaying` for hot-path gating.

```mermaid
flowchart LR
  subgraph Core["Core (코어)"]
    GM["GameManager<br>State hub: IsPlaying<br>Emits: OnGameStartAction, OnGameOverEvent"]
  end

  subgraph Gameplay["Gameplay (게임플레이)"]
    SP["Spawner<br>Starts/stops coroutine based on IsPlaying<br>Activates pooled objects (SetActive)"]
    BL["Bullet<br>Moves every frame<br>Lifetime ends by SetActive(false)<br>Destroy path disabled"]
    TR["Terrain<br>Deactivates when out of bounds (SetActive(false))"]
  end

  subgraph UI["UI (유아이)"]
    UIB["FetchAndSetGameState (Button)<br>OnClick toggles game state<br>Subscribes to game-over/start to show/hide"]
  end

  subgraph Pooling["Pooling (풀링)"]
    OM["ObjectManager (missing source file)<br>Spawner expects EnemyPrefabs / TerrainPrefabs lists"]
  end

  UIB -->|"\"ChangeGameState()\" call"| GM
  GM -->|"\"OnGameStartAction\" (C# event)"| UIB
  GM -->|"\"OnGameOverEvent\" (UnityEvent)"| UIB
  GM -->|"\"OnGameOverEvent\" (UnityEvent)"| SP

  SP -->|"\"EnemyPrefabs / TerrainPrefabs\" access"| OM
  SP -->|"\"SetActive(true/false)\" reuse"| TR
  BL -->|"\"SetActive(false)\" on timeout"| BL
```

### Repository notes (integrity + reproducibility)

- **Missing-but-referenced scripts**: `Spawner` references `ObjectManager.Instance.EnemyPrefabs/TerrainPrefabs`, but only `ObjectManager.cs.meta` is present (no `ObjectManager.cs`). Some other referenced classes (e.g., `Enemy`, `Weapon`, `Player`) appear as `.meta` without source.
  - **Impact**: The runtime architecture is still inferable from call sites, but full pooling initialization details are not auditable from this repo snapshot.

### Engineering standard (how performance is enforced)

- **Reuse-first gameplay loop**: Spawning and lifetime management prefer `SetActive(false)` over `Destroy()` to avoid allocator pressure and reduce hitch risk.
- **Event-driven state transitions**: `GameManager` emits start/over signals; subscribers bind/unbind to keep scene reloads stable.
- **Hot-path gating**: systems check `GameManager.Instance.IsPlaying` and only run coroutines/logic when active.

### ADRs (exactly two)

#### ADR-001 — Bullet lifetime: disable & reuse (not Destroy)

- **Context (problem)**: Frequent bullet expiry can create per-frame object churn and GC/CPU spikes when `Destroy()` is used on short-lived objects.
- **Decision (tech chosen)**: Bullet expiry uses `gameObject.SetActive(false)`; the `Destroy(gameObject)` path is intentionally disabled in `Assets/Scripts/Attack/Bullet.cs`.
- **Consequences (quantitative result vs trade-off)**:
  - **Result (repo-derived)**: `Destroy()` calls on bullet expiry reduced from **1 → 0** per expiry path (100% reduction on that hot path).
  - **Trade-off**: Requires a producer (pool/spawner) to reactivate/reposition bullets; otherwise deactivated instances accumulate and must be managed.

#### ADR-002 — Game state propagation: explicit events (Action/UnityEvent) vs polling-only coupling

- **Context (problem)**: Systems that poll global state in `Update()` tend to spread coupling and create hard-to-audit lifecycle leaks (especially UI and scene transitions).
- **Decision (tech chosen)**: `GameManager` is the state hub and exposes **`OnGameStartAction`** (C# event) and **`OnGameOverEvent`** (UnityEvent). Subscribers register in `Start()` and unsubscribe in `OnDestroy()` where appropriate (e.g., `FetchAndSetGameState`).
- **Consequences (quantitative result vs trade-off)**:
  - **Result (repo-derived)**: At least **2 distinct subscribers** are wired through the hub (`Spawner.DisableAll`, `FetchAndSetGameState` show/hide), enabling fan-out on a single state transition without adding new direct references between those modules.
  - **Trade-off**: Event wiring must be maintained carefully (subscribe/unsubscribe symmetry); missing unsubscriptions can keep callbacks alive across scene changes.

---

## 한국어 (Korean)

![Gameplay GIF](docs/gameplay.gif)

[![Unity](https://img.shields.io/badge/Unity-2022.3.61f1-black?logo=unity)](#)
[![TextMeshPro](https://img.shields.io/badge/TMP-3.0.7-0aa?logo=unity)](#)
[![Timeline](https://img.shields.io/badge/Timeline-1.7.7-0aa?logo=unity)](#)
[![VisualScripting](https://img.shields.io/badge/Visual%20Scripting-1.9.4-0aa?logo=unity)](#)
[![ToonShader](https://img.shields.io/badge/Toon%20Shader-0.12.0--preview-0aa?logo=unity)](#)

Unity 기반의 bullet-storm 스타일 프로젝트로, **이벤트 기반 게임 상태 전파**와 **재사용 우선 런타임 동작**(핫패스에서 오브젝트 churn/할당 최소화)을 중심으로 구성되어 있습니다.

### 빠른 시작

- **Unity**: `2022.3.61f1` (출처: `ProjectSettings/ProjectVersion.txt`)
- **실행**: Unity Hub → Open → Play in Editor

### Key Optimization Performance

> 리포지토리에 프로파일러 로그가 없을 경우, 아래 표는 **repo-derived**(코드 경로/카운트로 증명 가능한) 근거를 사용합니다. “Before”가 측정값이 아니라 베이스라인 가정인 경우 명시합니다.

| Measurement Metric | Before | After | Result (%) |
|---|---:|---:|---:|
| 총알 수명 종료에서 `Destroy()` 사용(오브젝트 churn) | 만료 1회당 1회 호출(레거시 경로가 주석으로 존재) | 만료 1회당 0회 호출(`Assets/Scripts/Attack/Bullet.cs`에서 `SetActive(false)` 사용) | 100 |
| 게임플레이 스크립트에서 스폰 타이밍 `Instantiate()` | 스폰 1회당 1회(일반적인 Unity 패턴; 측정값 아님) | `Assets/Scripts/**/*.cs`에서 0건(repo-derived) | 100 |

### 아키텍처 (리포지토리에서 관찰된 구조)

런타임 연결은 **상태 허브** 역할의 `GameManager`를 중심으로 이벤트를 발생시키고, 각 시스템이 이를 구독하는 형태입니다. UI와 스포너는 상태 변화에 따라 활성/비활성 및 루틴을 제어합니다.

```mermaid
flowchart LR
  subgraph Core["코어 (Core)"]
    GM["GameManager<br>State hub: IsPlaying<br>Emits: OnGameStartAction, OnGameOverEvent"]
  end

  subgraph Gameplay["게임플레이 (Gameplay)"]
    SP["Spawner<br>IsPlaying에 따라 코루틴 시작/중지<br>SetActive로 오브젝트 재사용"]
    BL["Bullet<br>프레임마다 이동<br>수명 종료 시 SetActive(false)<br>Destroy 경로 비활성화"]
    TR["Terrain<br>범위 이탈 시 SetActive(false)"]
  end

  subgraph UI["유아이 (UI)"]
    UIB["FetchAndSetGameState (Button)<br>클릭 시 게임 상태 토글<br>GameOver/Start 구독으로 표시/숨김"]
  end

  subgraph Pooling["풀링 (Pooling)"]
    OM["ObjectManager (소스 누락)<br>Spawner는 EnemyPrefabs / TerrainPrefabs 리스트를 기대"]
  end

  UIB -->|"\"ChangeGameState()\" 호출"| GM
  GM -->|"\"OnGameStartAction\" (C# event)"| UIB
  GM -->|"\"OnGameOverEvent\" (UnityEvent)"| UIB
  GM -->|"\"OnGameOverEvent\" (UnityEvent)"| SP

  SP -->|"\"EnemyPrefabs / TerrainPrefabs\" 접근"| OM
  SP -->|"\"SetActive(true/false)\" 재사용"| TR
  BL -->|"\"SetActive(false)\" (타임아웃)"| BL
```

### 리포지토리 상태 메모 (재현성/무결성)

- **참조되지만 소스가 없는 스크립트**: `Spawner`는 `ObjectManager.Instance.EnemyPrefabs/TerrainPrefabs`에 의존하지만 `ObjectManager.cs`는 없고 `.meta`만 존재합니다. (`Enemy`, `Weapon`, `Player` 등도 일부가 `.meta`만 존재)
  - **영향**: 호출부 기반으로 구조/의도는 파악 가능하지만, 풀 초기화/프리워밍 방식은 현재 스냅샷만으로는 검증할 수 없습니다.

### 개발 표준(성능을 “말”이 아니라 “구조”로 보장)

- **재사용 우선 루프**: `Destroy()` 대신 `SetActive(false)` 기반으로 수명 종료를 설계하여 hitch 위험을 줄입니다.
- **이벤트 기반 상태 전파**: `GameManager`가 시작/종료 이벤트를 발행하고, 구독자는 명시적으로 바인딩/언바인딩합니다.
- **핫패스 게이팅**: `IsPlaying`을 기준으로 불필요한 코루틴/로직 실행을 제한합니다.

### ADRs

ADRs는 문서 전체에서 **정확히 2개**만 유지합니다. (영문 섹션의 `ADR-001`, `ADR-002` 참고)

---

## Cursor AI generated disclaimer

This `README.md` was generated by Cursor AI based on the contents present in the repository at generation time. Any “repo-derived” claims are grounded in observable files and code paths; if performance logs, missing scripts, or external assets exist outside this snapshot, conclusions may change after reconciliation.
