# Система врагов для UnityRia

## Описание
Полная система врагов с State Machine, патрулированием, преследованием и атаками

## Структура файлов

### Интерфейсы
- `Enemies/Interfaces/IDamaging.cs` - интерфейс для объектов, наносящих урон
- `Enemies/Interfaces/IDamageable.cs` - интерфейс для объектов, получающих урон

### State Machine
- `StateMachine/State.cs` - базовый класс для всех состояний
- `StateMachine/Transition.cs` - базовый класс для всех переходов
- `StateMachine/StateMachineController.cs` - основной контроллер State Machine

### Состояния (States)
- `StateMachine/States/CommonStates/PatrolState.cs` - патрулирование между точками
- `StateMachine/States/CommonStates/PursuitState.cs` - преследование игрока
- `StateMachine/States/CommonStates/IdleState.cs` - состояние бездействия
- `StateMachine/States/CombatStates/MeleeAttackState.cs` - ближняя атака

### Переходы (Transitions)
- `StateMachine/Transitions/CommonTransitions/DetectTransition.cs` - обнаружение игрока
- `StateMachine/Transitions/CommonTransitions/PlayerExitDetectionTransition.cs` - игрок вышел из зоны
- `StateMachine/Transitions/CommonTransitions/AnimationEndTransition.cs` - окончание анимации
- `StateMachine/Transitions/CombatTransitions/AttackRangeTransition.cs` - игрок в зоне атаки
- `StateMachine/Transitions/CombatTransitions/OutOfAttackRangeTransition.cs` - игрок вне зоны атаки

### Враги
- `Enemies/Enemy.cs` - базовый класс врага с уроном при столкновении

## Как настроить врага в Unity

### Шаг 1: Создание объекта врага
1. Создайте GameObject для врага
2. Добавьте компоненты:
   - `SpriteRenderer` (для отображения)
   - `Animator` (для анимаций)
   - `PolygonCollider2D` (для коллизии)
   - `StateMachineController` (обязательно!)
   - `Enemy` (базовый класс врага)

**⚠️ ВАЖНО: Как правильно добавить компоненты**

#### Добавление компонента StateMachineController:
1. Выберите ваш GameObject врага
2. В Inspector нажмите кнопку **"Add Component"**
3. В поиске введите **"StateMachineController"** (или просто **"StateMachine"**)
4. **НЕ выбирайте** "State Machine" из Visual Scripting (у него есть поля "Source", "Graph", "Convert")
5. Выберите компонент **"State Machine Controller"** из папки `StateMachine` (должен быть в списке скриптов)
6. После добавления вы должны увидеть поле **"First State"** в Inspector

#### Добавление компонента Enemy:
1. В Inspector нажмите кнопку **"Add Component"**
2. В поиске введите **"Enemy"**
3. Выберите компонент **"Enemy"** из папки `Enemies`
4. После добавления вы должны увидеть поле **"Enable Collider Delay"** в Inspector

**Если поля не отображаются:**
- Убедитесь, что вы добавили правильные компоненты (не Visual Scripting)
- Проверьте, что скрипты скомпилированы без ошибок (окно Console)
- Попробуйте перезагрузить сцену или перезапустить Unity

### Шаг 2: Настройка State Machine
1. Убедитесь, что компонент `StateMachineController` добавлен (см. инструкции выше)
2. **Пока оставьте поле "First State" пустым** - мы заполним его после добавления состояния в Шаге 3

### Шаг 3: Настройка PatrolState (Патрулирование)
1. **Добавьте компонент `PatrolState`:**
   - В Inspector нажмите **"Add Component"**
   - В поиске введите **"PatrolState"**
   - Выберите компонент из папки `StateMachine/States/CommonStates`
2. **Теперь вернитесь к компоненту `StateMachineController`:**
   - В поле **"First State"** перетащите компонент `PatrolState` (или выберите его из списка)
3. **Создайте 2+ пустых GameObject'ов** как точки патрулирования (Waypoints)
4. **В инспекторе `PatrolState`** настройте:
   - **Waypoints**: Перетащите созданные точки патрулирования
   - **Length Patrol Route**: Длина маршрута (по умолчанию 1.7)
   - **Speed**: Скорость движения (по умолчанию 2.0)
   - **Move Direction**: Horizontal (горизонтально) или Vertical (вертикально)
   - **Need Update Waypoint Position**: Если включено, точки обновляются автоматически

### Шаг 4: Настройка DetectTransition (Обнаружение игрока)
1. Добавьте компонент `DetectTransition` в список Transitions у `PatrolState`
2. Настройте:
   - **Target State**: `PursuitState` (состояние преследования)
   - **Detection Zone Type**: Circle (круг) или Rectangle (прямоугольник)
   - **Detection Radius**: Радиус обнаружения (по умолчанию 5)
   - **Target Layer**: Слой, на котором находится игрок (обычно "Player")
   - **Detect Reaction Delay**: Задержка перед реакцией

### Шаг 5: Настройка PursuitState (Преследование)
1. Добавьте компонент `PursuitState`
2. Настройте:
   - **Speed**: Скорость преследования (по умолчанию 2.0)

### Шаг 6: Настройка AttackRangeTransition
1. Добавьте компонент `AttackRangeTransition` в список Transitions у `PursuitState`
2. Настройте:
   - **Target State**: `MeleeAttackState`
   - **Attack Range**: Дистанция атаки (по умолчанию 1.5)
   - **Target Layer**: Слой игрока

### Шаг 7: Настройка MeleeAttackState (Атака)
1. Добавьте компонент `MeleeAttackState`
2. Создайте пустой GameObject как `Raycast Origin` (точка начала луча атаки)
3. Настройте:
   - **Raycast Origin**: Перетащите созданный GameObject
   - **Raycast Length**: Длина луча атаки
   - **Raycast Origin Offset X**: Смещение по X

### Шаг 8: Настройка анимаций
В Animator Controller создайте параметры:
- `Patrol` (Trigger) - для анимации патрулирования
- `Attack` (Trigger) - для анимации атаки
- `Idle` (Trigger) - для анимации бездействия

В анимации атаки добавьте Animation Event, вызывающий метод `Attack()` из `MeleeAttackState`.

### Шаг 9: Настройка Enemy
1. Добавьте компонент `Enemy`
2. Настройте:
   - **Enable Collider Delay**: Задержка перед повторным включением коллайдера (по умолчанию 1.5)

## Пример настройки полной цепочки состояний

```
PatrolState
  └─ DetectTransition → PursuitState
       └─ AttackRangeTransition → MeleeAttackState
            └─ OutOfAttackRangeTransition → PursuitState
                 └─ PlayerExitDetectionTransition → PatrolState
```

## Важные замечания

1. **PlayerController**: Убедитесь, что у игрока есть компонент `PlayerController` с публичным полем `moveable`
2. **Layers**: Настройте Layer для игрока и укажите его в `Target Layer` переходов
3. **Colliders**: Убедитесь, что у врага есть `PolygonCollider2D` с `Is Trigger = true` для обнаружения столкновений
4. **Gizmos**: В Scene View можно визуализировать зоны обнаружения и атаки (красный/синий/желтый/магента)

## Дополнительные возможности

- Можно добавить больше состояний (JumpAttackState, ShootState и т.д.)
- Можно создать кастомные переходы для специфичных условий
- Система легко расширяется новыми состояниями и переходами

