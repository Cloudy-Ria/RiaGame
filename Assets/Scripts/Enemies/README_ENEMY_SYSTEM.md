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
   - `StateMachineController` (обязательно!)
   - `Enemy` (базовый класс врага)
   
**Примечание:** `BoxCollider2D` будет автоматически добавлен при добавлении компонента `PatrolState` (см. Шаг 3). Этот же коллайдер используется компонентом `Enemy` для обнаружения столкновений.

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

**Если появляется ошибка Burst "Failed to resolve assembly: 'Assembly-CSharp-Editor'":**
- Это предупреждение компилятора Burst, обычно не критично и не мешает работе проекта
- Если ошибка мешает, попробуйте очистить кеш Unity:
  1. Закройте Unity
  2. Удалите папки `Library` и `Temp` в корне проекта
  3. Откройте проект заново - Unity пересоберёт кеш

### Шаг 2: Настройка State Machine
1. Убедитесь, что компонент `StateMachineController` добавлен (см. инструкции выше)
2. **Пока оставьте поле "First State" пустым** - мы заполним его после добавления состояния в Шаге 3

### Шаг 3: Настройка PatrolState (Патрулирование)
1. **Добавьте компонент `PatrolState`:**
   - В Inspector нажмите **"Add Component"**
   - В поиске введите **"PatrolState"**
   - Выберите компонент из папки `StateMachine/States/CommonStates`
   - **Примечание:** При добавлении `PatrolState` автоматически добавится `BoxCollider2D` (не нужно добавлять вручную)
2. **Теперь вернитесь к компоненту `StateMachineController`:**
   - В поле **"First State"** перетащите компонент `PatrolState` (или выберите его из списка)
3. **Создайте 2+ пустых GameObject'ов как дочерние объекты врага** (точки патрулирования - Waypoints):
   - В Hierarchy выберите GameObject врага
   - Кликните правой кнопкой на враге → **Create Empty** (создайте 2 или больше пустых объектов)
   - **Важно:** Waypoints должны быть дочерними объектами врага (вложены в него в Hierarchy)
   - Переименуйте их понятными именами (например, `Waypoint1`, `Waypoint2`)
   - Расположите их в нужных позициях относительно врага (враг будет двигаться между этими точками)
   - **Преимущество:** При перемещении врага waypoints автоматически двигаются вместе с ним, нужно только детально настраивать их позиции
4. **В инспекторе `PatrolState`** настройте:
   - **Waypoints**: Перетащите созданные дочерние точки патрулирования в этот массив (можно перетащить все сразу или по одной)
   - **Speed**: Скорость движения (по умолчанию 2.0)
   - **Move Direction**: Horizontal (горизонтально) или Vertical (вертикально)
   - **Важно:** Waypoints статичны - их позиции задаются вручную и ограничивают зону патрулирования врага

### Шаг 4: Настройка PursuitState (Преследование)
1. **Добавьте компонент `PursuitState`:**
   - В Inspector нажмите **"Add Component"**
   - В поиске введите **"PursuitState"**
   - Выберите компонент из папки `StateMachine/States/CommonStates`
2. **Настройте `PursuitState`:**
   - **Speed**: Скорость преследования (по умолчанию 2.0)

### Шаг 5: Настройка DetectTransition (Обнаружение игрока)
1. **Добавьте компонент `DetectTransition` на GameObject врага:**
   - В Inspector нажмите **"Add Component"**
   - В поиске введите **"DetectTransition"**
   - Выберите компонент из папки `StateMachine/Transitions/CommonTransitions`
2. **Перетащите `DetectTransition` в список Transitions у `PatrolState`:**
   - В Inspector найдите компонент `PatrolState`
   - Найдите поле **"Transitions"** (список)
   - Перетащите компонент `DetectTransition` из списка компонентов в это поле
3. **Настройте `DetectTransition`:**
   - **Target State**: Перетащите компонент `PursuitState` (состояние преследования)
   - **Detection Zone Type**: Circle (круг) или Rectangle (прямоугольник)
   - **Detection Radius**: Радиус обнаружения (по умолчанию 5)
   - **Target Layer**: Слой, на котором находится игрок (обычно "Player")
   - **Detect Reaction Delay**: Задержка перед реакцией

### Шаг 6: Настройка AttackRangeTransition
1. **Добавьте компонент `AttackRangeTransition` на GameObject врага:**
   - В Inspector нажмите **"Add Component"**
   - В поиске введите **"AttackRangeTransition"**
   - Выберите компонент из папки `StateMachine/Transitions/CombatTransitions`
2. **Перетащите `AttackRangeTransition` в список Transitions у `PursuitState`:**
   - В Inspector найдите компонент `PursuitState`
   - Найдите поле **"Transitions"** (список)
   - Перетащите компонент `AttackRangeTransition` из списка компонентов в это поле
3. **Настройте `AttackRangeTransition`:**
   - **Target State**: Перетащите компонент `MeleeAttackState`
   - **Attack Range**: Дистанция атаки (по умолчанию 1.5)
   - **Target Layer**: Слой игрока

### Шаг 7: Настройка MeleeAttackState (Атака)
1. **Добавьте компонент `MeleeAttackState`:**
   - В Inspector нажмите **"Add Component"**
   - В поиске введите **"MeleeAttackState"**
   - Выберите компонент из папки `StateMachine/States/CombatStates`
2. **Создайте пустой GameObject как `Raycast Origin`:**
   - В Hierarchy кликните правой кнопкой на GameObject врага → **Create Empty**
   - Переименуйте созданный GameObject в `RaycastOrigin` (или другое понятное имя)
   - Расположите его в нужной позиции относительно врага (например, на уровне руки или оружия)
   - **Важно:** Этот GameObject используется как точка начала луча атаки - от него будет идти проверка попадания в игрока
3. **Настройте `MeleeAttackState`:**
   - **Raycast Origin**: Перетащите созданный GameObject `RaycastOrigin` в это поле
   - **Raycast Length**: Длина луча атаки (на какое расстояние враг может атаковать, например: 1.5)
   - **Raycast Origin Offset X**: Смещение по X при повороте спрайта (обычно 0, если точка атаки симметрична)
     - **Что задавать:** Числовое значение смещения в единицах Unity (например: 0.5, -0.3, или 0)
     - **Когда нужно:** Если при повороте врага точка атаки смещается, укажите значение для компенсации
     - **Пример:** Если точка атаки смещается на 0.5 единицы вправо при повороте влево, укажите `0.5`
     - **По умолчанию:** `0` (точка атаки симметрична и не требует смещения)

### Шаг 8: Настройка анимаций

#### 8.1. Создание Animator Controller (если его еще нет)
1. В Project окне кликните правой кнопкой → **Create** → **Animator Controller**
2. Переименуйте его (например, `EnemyAnimatorController`)
3. Перетащите его в поле **Controller** компонента `Animator` у врага

#### 8.2. Создание параметров в Animator Controller
1. Откройте Animator Controller (двойной клик на файле контроллера)
2. В окне **Animator** найдите панель **Parameters** (слева вверху)
3. Нажмите **"+"** → выберите **Trigger**
4. Создайте три параметра (именно с такими именами!):
   - `Patrol` (Trigger) - для анимации `Enemy_fly` (патрулирование)
   - `Attack` (Trigger) - для анимации `Enemy_attack` (атака)
   - **Примечание:** Для `Enemy_fall` (преследование) параметр не требуется, так как эта анимация не используется в текущей системе

#### 8.3. Создание состояний анимаций
1. **Откройте Animator Controller** (двойной клик на файле контроллера)
2. **Создайте состояние для патрулирования (`Enemy_fly`):**
   - В окне **Project** найдите файл `Assets/Animations/Enemies/Enemy_fly.anim`
   - Перетащите этот файл анимации в окно **Animator** (в пустое место справа от дефолтного состояния)
   - Unity автоматически создаст состояние с именем `Enemy_fly`
   - Переименуйте состояние (кликните правой кнопкой на состоянии → **Rename**) в `Patrol` (для удобства)
3. **Создайте состояние для атаки (`Enemy_attack`):**
   - В окне **Project** найдите файл `Assets/Animations/Enemies/Enemy_attack.anim`
   - Перетащите этот файл анимации в окно **Animator**
   - Переименуйте состояние в `Attack` (для удобства)
4. **Настройте дефолтное состояние:**
   - Выберите состояние `Patrol` (Enemy_fly)
   - Кликните правой кнопкой → **Set as Layer Default State** (это будет начальное состояние)

#### 8.4. Настройка переходов между состояниями
1. **Переход в патрулирование (Patrol):**
   - Выберите состояние **Any State** (желтое состояние вверху)
   - Кликните правой кнопкой → **Make Transition** → выберите состояние `Patrol` (Enemy_fly)
   - В Inspector перехода найдите **Conditions**
   - Нажмите **"+"** → выберите параметр `Patrol` (Trigger)
   - Отключите **Has Exit Time** (снимите галочку)
   - Установите **Transition Duration** на `0`
2. **Переход в атаку (Attack):**
   - Выберите состояние **Any State**
   - Кликните правой кнопкой → **Make Transition** → выберите состояние `Attack` (Enemy_attack)
   - В Inspector перехода найдите **Conditions**
   - Нажмите **"+"** → выберите параметр `Attack` (Trigger)
   - Отключите **Has Exit Time**
   - Установите **Transition Duration** на `0`
3. **Возврат из атаки в патрулирование:**
   - Выберите состояние `Attack` (Enemy_attack)
   - Кликните правой кнопкой → **Make Transition** → выберите состояние `Patrol` (Enemy_fly)
   - В Inspector перехода:
     - **Не добавляйте условий** (переход произойдет автоматически после окончания анимации)
     - Включите **Has Exit Time** (поставьте галочку)
     - Установите **Exit Time** на `1` (переход в конце анимации)
     - Установите **Transition Duration** на `0.1` (небольшая задержка для плавности)

#### 8.5. Добавление Animation Event в анимацию атаки
1. **Откройте анимацию атаки:**
   - В окне **Project** найдите файл `Assets/Animations/Enemies/Enemy_attack.anim`
   - Двойной клик на файле, чтобы открыть его в окне **Animation**
2. **Найдите момент атаки:**
   - В окне **Animation** на временной шкале найдите момент, когда должна происходить атака (например, когда рука/оружие/клюв достигает цели)
   - Переместите красную линию (playhead) на нужный момент
3. **Добавьте Animation Event:**
   - В панели **Animation Events** (внизу окна Animation, под временной шкалой) нажмите кнопку **"+"** для добавления события
   - На временной шкале появится белый треугольник - это маркер события
4. **Настройте событие:**
   - Выберите созданное событие (белый треугольник)
   - В Inspector (справа) в поле **Function** введите: `Attack`
   - **Важно:** Unity автоматически найдет метод `Attack()` в компоненте `MeleeAttackState` на том же GameObject, что и Animator
   - Поле **Object** можно оставить пустым
5. **Сохраните изменения:**
   - Нажмите **Ctrl+S** или **File → Save**

**Примечание:** 
- Метод `Attack()` вызывается автоматически из `MeleeAttackState` при срабатывании Animation Event
- Убедитесь, что компонент `MeleeAttackState` добавлен на тот же GameObject, что и Animator
- Если анимация `Enemy_fall` не используется в текущей системе, её можно не добавлять в Animator Controller

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
3. **Colliders**: 
   - `BoxCollider2D` автоматически добавляется при добавлении `PatrolState` (используется для смещения коллайдера при движении и для обнаружения столкновений с игроком)
   - Убедитесь, что коллайдер имеет `Is Trigger = true` для обнаружения столкновений с игроком
4. **Gizmos**: В Scene View можно визуализировать зоны обнаружения и атаки (красный/синий/желтый/магента)

## Дополнительные возможности

- Можно добавить больше состояний (JumpAttackState, ShootState и т.д.)
- Можно создать кастомные переходы для специфичных условий
- Система легко расширяется новыми состояниями и переходами

