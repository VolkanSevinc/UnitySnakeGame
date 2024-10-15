using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GoalUIElement : MonoBehaviour
    {
        [SerializeField] private Image goalTypeImage;
        [SerializeField] private TextMeshProUGUI goalCountText;

        private int _targetGoalCount;

        public void Init(Sprite goalSprite, int currentAmount, int targetGoalCount)
        {
            goalTypeImage.sprite = goalSprite;
            _targetGoalCount = targetGoalCount;
            UpdateGoalCount(currentAmount);
        }

        public void UpdateGoalCount(int newGoalCount)
        {
            goalCountText.text = newGoalCount + "/" + _targetGoalCount;
        }
    }
}