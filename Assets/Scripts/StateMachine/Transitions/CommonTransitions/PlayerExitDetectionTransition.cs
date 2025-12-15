using StateMachine.TriggersAndZones;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Переход, который срабатывает, когда игрок ВЫХОДИТ из зоны обнаружения.
    /// Должен использовать тот же тип зоны (Circle / Rectangle),
    /// что и DetectTransition, чтобы тот самый прямоугольник
    /// реально ограничивал зону преследования.
    /// </summary>
    public class PlayerExitDetectionTransition : Transition
    {
        [SerializeField] private DetectionZoneType _detectionZoneType;
        [SerializeField] private DetectionZoneMovementType _zoneMovementType;
        [SerializeField] private float _detectionRadius = 5f;
        [SerializeField] private Vector2 _detectionRectangleSize = new Vector2(10f, 5f);
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _offsetY;
        [SerializeField] private float _offsetX;

        private Vector2 _fixedDetectionPosition;
        private bool _playerWasDetected = false;

        private void Awake()
        {
            if (_zoneMovementType == DetectionZoneMovementType.Static)
            {
                _fixedDetectionPosition = transform.parent.position;
            }
        }

        private void Update()
        {
            bool playerInRange = CheckPlayerInRange();

            if (playerInRange)
            {
                // Игрок внутри зоны – запоминаем, что он был обнаружен.
                _playerWasDetected = true;
            }
            else if (_playerWasDetected)
            {
                // Игрок БЫЛ внутри зоны, но вышел – выполняем переход.
                NeedTransit = true;
                _playerWasDetected = false;
            }
        }

        private bool CheckPlayerInRange()
        {
            Vector2 offset = new Vector2(_offsetX, _offsetY);
            Vector2 adjustedPosition;

            switch (_zoneMovementType)
            {
                case DetectionZoneMovementType.Moving:
                    adjustedPosition = (Vector2)transform.position + offset;
                    break;
                case DetectionZoneMovementType.Static:
                    adjustedPosition = _fixedDetectionPosition + offset;
                    break;
                default:
                    adjustedPosition = _fixedDetectionPosition + offset;
                    break;
            }

            Collider2D hit;

            if (_detectionZoneType == DetectionZoneType.Circle)
            {
                hit = Physics2D.OverlapCircle(adjustedPosition, _detectionRadius, _targetLayer);
            }
            else // DetectionZoneType.Rectangle
            {
                hit = Physics2D.OverlapBox(adjustedPosition, _detectionRectangleSize, 0f, _targetLayer);
            }

            return hit != null && hit.GetComponent<PlayerController>() != null;
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 offset = new Vector2(_offsetX, _offsetY);
            Vector2 adjustedPosition;

            switch (_zoneMovementType)
            {
                case DetectionZoneMovementType.Moving:
                    adjustedPosition = (Vector2)transform.position + offset;
                    break;
                case DetectionZoneMovementType.Static:
                    adjustedPosition = _fixedDetectionPosition + offset;
                    break;
                default:
                    adjustedPosition = _fixedDetectionPosition + offset;
                    break;
            }

            Gizmos.color = Color.yellow;

            if (_detectionZoneType == DetectionZoneType.Circle)
            {
                Gizmos.DrawWireSphere(adjustedPosition, _detectionRadius);
            }
            else // Rectangle
            {
                Gizmos.DrawWireCube(adjustedPosition, _detectionRectangleSize);
            }
        }
    }
}

