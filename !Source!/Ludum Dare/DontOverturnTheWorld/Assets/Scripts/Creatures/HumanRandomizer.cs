using System.Collections.Generic;
using UnityEngine;


namespace Creatures
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class HumanRandomizer : MonoBehaviour
    {
        public float SpeedRandomness;
        public Sprite[] HumanSprites;
        public SpriteRenderer body;

        private Human _human;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _human = GetComponentInParent<Human>();
            _human.MotionSpeed *= Random.Range(1 - SpeedRandomness, 1 + SpeedRandomness);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = HumanSprites[Random.Range(0, HumanSprites.Length)];

            // Animation animation = GetComponentInParent<Animation>();
            // animation. // FIXME
            var colors = new List<Color>
            {
                new Color32(205, 187, 144, 255),
                new Color32(142, 105, 74, 255),
                new Color32(219, 167, 158, 255),
                new Color32(215, 215, 215, 255),
                new Color32(138, 111, 111, 255),
                new Color32(145, 55, 55, 255)
            };

            body.color = colors[Random.Range(0, colors.Count)];
        }
    }
}
