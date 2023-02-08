using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TwistEffect : MonoBehaviour
{
    public Vector2 radius = new Vector2(0.5F, 0.5F);
    public float angle = 0;
    public Vector2 center = new Vector2(0.5F, 0.5F);

    public float speed = 500f;
    public float maxAngle = 1000;
    public float minAngle = -1000;

    Material material;

    void OnEnable()
    {
        material = GetComponent<Image>().material;
        material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
        material.SetFloat("_Angle", angle * Mathf.Deg2Rad);
        //StartCoroutine(SetTarget());
    }

    void Update()
    {
        angle += Time.deltaTime * speed;

        if (angle > maxAngle || angle < minAngle)
        {
            speed = -speed;
            angle += Time.deltaTime * speed;
        }

        UpdateShader();
    }

    IEnumerator SetTarget()
    {
        yield return new WaitForEndOfFrame();

        var rectTransform = transform as RectTransform;

        //计算自身的屏幕坐标和大小
        var width = (int)rectTransform.rect.width * Screen.width / 1000;
        var height = (int)rectTransform.rect.height * Screen.width / 1000;
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x -= width / 2;
        pos.y -= height / 2;

        ////生成屏幕区域的Texture
        //var tex = new Texture2D(width, height);
        //tex.ReadPixels(new Rect((int)pos.x, (int)pos.y, width, height), 0, 0);
        //tex.Apply();

        var rectT = rectTransform;

        //Texture2D shot = new Texture2D((int)(rect.rect.width), (int)(rect.rect.height));
        //float x = rect.localPosition.x + (Screen.width - rect.rect.width) * 0.5f;
        //float y = rect.localPosition.y + (Screen.height - rect.rect.height) * 0.5f;

        //Rect position = new Rect(x, y, rect.rect.width, rect.rect.height);
        //shot.ReadPixels(position, 0, 0);
        //shot.Apply();

        Texture2D t = new Texture2D((int)rectT.rect.width, (int)rectT.rect.height, TextureFormat.RGB24, true);//需要正确设置好图片保存格式
        float x = rectT.localPosition.x + (Screen.width - rectT.rect.width) / 2;
        float y = rectT.localPosition.y + (Screen.height - rectT.rect.height) / 2;
        Rect position = new Rect(x, y, rectT.rect.width, rectT.rect.height);
        t.ReadPixels(position, 0, 0, true);//按照设定区域读取像素；注意是以左下角为原点读取
        t.Apply();




        //将Texture赋值给Image
        Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
        var img = GetComponent<Image>();
        img.sprite = sprite;
        img.enabled = true;
    }

    void UpdateShader()
    {
        material.SetFloat("_Angle", angle * Mathf.Deg2Rad);
    }

    
}
