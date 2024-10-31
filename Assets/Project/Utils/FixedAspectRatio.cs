using UnityEngine;

public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 16.0f / 9.0f;  // Соотношение сторон, например 16:9

    void Start()
    {
        SetFixedAspectRatio();
    }

    void SetFixedAspectRatio()
    {
        // Текущее соотношение сторон экрана
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Коэффициент изменения, чтобы достичь нужного соотношения
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        // Если текущее соотношение больше целевого, добавим черные полосы сверху и снизу
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else // Если текущее соотношение меньше целевого, добавим полосы слева и справа
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
