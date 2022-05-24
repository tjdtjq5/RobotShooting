using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Function
{
    public static class MyMath
    {
        public static string Add(string A, string B)
        {

            if (A.Contains('.'))
            {
                Debug.Log(A);
                int len = A.IndexOf('.');
                A = A.Substring(0, len);
            }
            if (B.Contains('.'))
            {
                Debug.Log(B);
                int len = B.IndexOf('.');
                B = B.Substring(0, len);
            }

            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Add(intA, intB).ToString();
        }
        public static string Sub(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Subtract(intA, intB).ToString();
        }
        public static string Multiple(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Multiply(intA, intB).ToString();
        }
        public static string Multiple(string A, float B)
        {
            BigInteger intA = BigInteger.Parse(A);
            B = B * 100;
            BigInteger intB = new BigInteger(B);

            return (BigInteger.Multiply(intA, intB) / 100).ToString();
        }
        public static string Multiple(float A, float B)
        {
            A = A * 100;
            BigInteger intA = new BigInteger(A);
            B = B * 100;
            BigInteger intB = new BigInteger(B);

            return (BigInteger.Multiply(intA, intB) / 10000).ToString();
        }

        public static string Divide(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Divide(intA, intB).ToString();
        }

        public static string Divide(string A, int B)
        {
            BigInteger intA = BigInteger.Parse(A);

            return (intA / B).ToString();
        }

        public static string Divide(string A, float B)
        {
            BigInteger intA = BigInteger.Parse(A);
            B = B * 100;
            BigInteger intB = new BigInteger(B);

            return (BigInteger.Divide(intA, intB) / 100).ToString();
        }

        public static float Amount(string small, string big)
        {
            BigInteger intA = BigInteger.Parse(small) * 100;
            BigInteger intB = BigInteger.Parse(big);

            int intAmount = int.Parse(BigInteger.Divide(intA, intB).ToString());
            return intAmount / (float)100;
        }

        public static int CompareValue(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            if (intA < intB)
            {
                return -1;
            }
            if (intA > intB)
            {
                return 1;
            }
            return 0;
        }
    }

    public class Tool
    {
        public static float GetAngle(UnityEngine.Vector2 start, UnityEngine.Vector2 end)
        {
            UnityEngine.Vector2 v2 = end - start;
            return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        }

        // 젤 가까운적 발견
        public static Transform SearchCharacter(float radius, UnityEngine.Vector2 pos, string characterTag)
        {
            RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(pos, radius, UnityEngine.Vector2.zero);

            for (int i = 0; i < raycastHit2Ds.Length; i++)
            {
                Transform character = raycastHit2Ds[i].transform.GetComponent<Transform>();
                if (character.tag == characterTag)
                {
                    return character;
                }
            }

            return null;
        }

        public static Transform SearchCharacter(UnityEngine.Vector2 rect, UnityEngine.Vector2 pos, string characterTag)
        {
            RaycastHit2D[] raycastHit2Ds = Physics2D.BoxCastAll(pos, rect, 0, UnityEngine.Vector2.zero);

            for (int i = 0; i < raycastHit2Ds.Length; i++)
            {
                Transform character = raycastHit2Ds[i].transform;
                if (character.tag == characterTag)
                {
                    return character;
                }
            }

            return null;
        }

        // 주변 적 모두 발견

        public static List<Transform> SearchCharacterList(float radius, UnityEngine.Vector2 pos, string characterTag)
        {
            List<Transform> characters = new List<Transform>();

            RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(pos, radius, UnityEngine.Vector2.zero);

            for (int i = 0; i < raycastHit2Ds.Length; i++)
            {
                Transform character = raycastHit2Ds[i].transform;
                if (character.tag == characterTag)
                {
                    characters.Add(character);
                }
            }

            return characters;
        }

        public static List<Transform> SearchCharacterList(UnityEngine.Vector2 rect, float angle, UnityEngine.Vector2 pos, string characterTag)
        {
            List<Transform> characters = new List<Transform>();
            RaycastHit2D[] raycastHit2Ds = Physics2D.BoxCastAll(pos, rect, angle, UnityEngine.Vector2.zero);

            for (int i = 0; i < raycastHit2Ds.Length; i++)
            {
                Transform character = raycastHit2Ds[i].transform;
                if (character.tag == characterTag)
                {
                    characters.Add(character);
                }
            }

            return characters;
        }
        public static IEnumerator ImageColorChangeCoroutine(Image image, Color originColor, Color changeColor, float speed)
        {
            bool r = false, g = false, b = false, a = false;
            float rColor = originColor.r;
            float gColor = originColor.g;
            float bColor = originColor.b;
            float aColor = originColor.a;
            WaitForSeconds waitTime = new WaitForSeconds(0.02f);

            while (!r || !g || !b || !a)
            {
                if (!r)
                {
                    if (rColor < changeColor.r)
                    {
                        rColor += speed;
                        if (changeColor.r - rColor <= speed) r = true;
                    }
                    else
                    {
                        rColor -= speed;
                        if (rColor - changeColor.r <= speed) r = true;
                    }
                }

                if (!g)
                {
                    if (gColor < changeColor.g)
                    {
                        gColor += speed;
                        if (changeColor.g - gColor <= speed) g = true;
                    }
                    else
                    {
                        gColor -= speed;
                        if (gColor - changeColor.g <= speed) g = true;
                    }
                }

                if (!b)
                {
                    if (bColor < changeColor.b)
                    {
                        bColor += speed;
                        if (changeColor.b - bColor <= speed) b = true;
                    }
                    else
                    {
                        bColor -= speed;
                        if (bColor - changeColor.b <= speed) b = true;
                    }
                }

                if (!a)
                {
                    if (aColor < changeColor.a)
                    {
                        aColor += speed;
                        if (changeColor.a - aColor <= speed) a = true;
                    }
                    else
                    {
                        aColor -= speed;
                        if (aColor - changeColor.a <= speed) a = true;
                    }
                }

                image.color = new Color(rColor, gColor, bColor, aColor);
                yield return waitTime;
            }
            while (r || g || b || a)
            {
                if (r)
                {
                    if (rColor < originColor.r)
                    {
                        rColor += speed;
                        if (originColor.r - rColor <= speed) r = false;
                    }
                    else
                    {
                        rColor -= speed;
                        if (rColor - originColor.r <= speed) r = false;
                    }
                }

                if (g)
                {
                    if (gColor < originColor.g)
                    {
                        gColor += speed;
                        if (originColor.g - gColor <= speed) g = false;
                    }
                    else
                    {
                        gColor -= speed;
                        if (gColor - originColor.g <= speed) g = false;
                    }
                }

                if (b)
                {
                    if (bColor < originColor.b)
                    {
                        bColor += speed;
                        if (originColor.b - bColor <= speed) b = false;
                    }
                    else
                    {
                        bColor -= speed;
                        if (bColor - originColor.b <= speed) b = false;
                    }
                }

                if (a)
                {
                    if (aColor < originColor.a)
                    {
                        aColor += speed;
                        if (originColor.a - aColor <= speed) a = false;
                    }
                    else
                    {
                        aColor -= speed;
                        if (aColor - originColor.a <= speed) a = false;
                    }
                }

                image.color = new Color(rColor, gColor, bColor, aColor);
                yield return waitTime;
            }
            image.color = originColor;
        }
        public static IEnumerator ImageColorChangeCoroutine(SpriteRenderer sprite, Color originColor, Color changeColor, float speed)
        {
            bool r = false, g = false, b = false, a = false;
            float rColor = originColor.r;
            float gColor = originColor.g;
            float bColor = originColor.b;
            float aColor = originColor.a;
            WaitForSeconds waitTime = new WaitForSeconds(0.02f);

            while (!r || !g || !b || !a)
            {
                if (!r)
                {
                    if (rColor < changeColor.r)
                    {
                        rColor += speed;
                        if (changeColor.r - rColor <= speed) r = true;
                    }
                    else
                    {
                        rColor -= speed;
                        if (rColor - changeColor.r <= speed) r = true;
                    }
                }

                if (!g)
                {
                    if (gColor < changeColor.g)
                    {
                        gColor += speed;
                        if (changeColor.g - gColor <= speed) g = true;
                    }
                    else
                    {
                        gColor -= speed;
                        if (gColor - changeColor.g <= speed) g = true;
                    }
                }

                if (!b)
                {
                    if (bColor < changeColor.b)
                    {
                        bColor += speed;
                        if (changeColor.b - bColor <= speed) b = true;
                    }
                    else
                    {
                        bColor -= speed;
                        if (bColor - changeColor.b <= speed) b = true;
                    }
                }

                if (!a)
                {
                    if (aColor < changeColor.a)
                    {
                        aColor += speed;
                        if (changeColor.a - aColor <= speed) a = true;
                    }
                    else
                    {
                        aColor -= speed;
                        if (aColor - changeColor.a <= speed) a = true;
                    }
                }

                sprite.color = new Color(rColor, gColor, bColor, aColor);
                yield return waitTime;
            }
            while (r || g || b || a)
            {
                if (r)
                {
                    if (rColor < originColor.r)
                    {
                        rColor += speed;
                        if (originColor.r - rColor <= speed) r = false;
                    }
                    else
                    {
                        rColor -= speed;
                        if (rColor - originColor.r <= speed) r = false;
                    }
                }

                if (g)
                {
                    if (gColor < originColor.g)
                    {
                        gColor += speed;
                        if (originColor.g - gColor <= speed) g = false;
                    }
                    else
                    {
                        gColor -= speed;
                        if (gColor - originColor.g <= speed) g = false;
                    }
                }

                if (b)
                {
                    if (bColor < originColor.b)
                    {
                        bColor += speed;
                        if (originColor.b - bColor <= speed) b = false;
                    }
                    else
                    {
                        bColor -= speed;
                        if (bColor - originColor.b <= speed) b = false;
                    }
                }

                if (a)
                {
                    if (aColor < originColor.a)
                    {
                        aColor += speed;
                        if (originColor.a - aColor <= speed) a = false;
                    }
                    else
                    {
                        aColor -= speed;
                        if (aColor - originColor.a <= speed) a = false;
                    }
                }

                sprite.color = new Color(rColor, gColor, bColor, aColor);
                yield return waitTime;
            }
            sprite.color = originColor;
        }
        public static IEnumerator ImageColorChangeCoroutine(Text text, Color originColor, Color changeColor, float speed)
        {
            bool r = false, g = false, b = false, a = false;
            float rColor = originColor.r;
            float gColor = originColor.g;
            float bColor = originColor.b;
            float aColor = originColor.a;
            WaitForSeconds waitTime = new WaitForSeconds(0.02f);

            while (!r || !g || !b || !a)
            {
                if (!r)
                {
                    if (rColor < changeColor.r)
                    {
                        rColor += speed;
                        if (changeColor.r - rColor <= speed) r = true;
                    }
                    else
                    {
                        rColor -= speed;
                        if (rColor - changeColor.r <= speed) r = true;
                    }
                }

                if (!g)
                {
                    if (gColor < changeColor.g)
                    {
                        gColor += speed;
                        if (changeColor.g - gColor <= speed) g = true;
                    }
                    else
                    {
                        gColor -= speed;
                        if (gColor - changeColor.g <= speed) g = true;
                    }
                }

                if (!b)
                {
                    if (bColor < changeColor.b)
                    {
                        bColor += speed;
                        if (changeColor.b - bColor <= speed) b = true;
                    }
                    else
                    {
                        bColor -= speed;
                        if (bColor - changeColor.b <= speed) b = true;
                    }
                }

                if (!a)
                {
                    if (aColor < changeColor.a)
                    {
                        aColor += speed;
                        if (changeColor.a - aColor <= speed) a = true;
                    }
                    else
                    {
                        aColor -= speed;
                        if (aColor - changeColor.a <= speed) a = true;
                    }
                }

                text.color = new Color(rColor, gColor, bColor, aColor);
                yield return waitTime;
            }
            while (r || g || b || a)
            {
                if (r)
                {
                    if (rColor < originColor.r)
                    {
                        rColor += speed;
                        if (originColor.r - rColor <= speed) r = false;
                    }
                    else
                    {
                        rColor -= speed;
                        if (rColor - originColor.r <= speed) r = false;
                    }
                }

                if (g)
                {
                    if (gColor < originColor.g)
                    {
                        gColor += speed;
                        if (originColor.g - gColor <= speed) g = false;
                    }
                    else
                    {
                        gColor -= speed;
                        if (gColor - originColor.g <= speed) g = false;
                    }
                }

                if (b)
                {
                    if (bColor < originColor.b)
                    {
                        bColor += speed;
                        if (originColor.b - bColor <= speed) b = false;
                    }
                    else
                    {
                        bColor -= speed;
                        if (bColor - originColor.b <= speed) b = false;
                    }
                }

                if (a)
                {
                    if (aColor < originColor.a)
                    {
                        aColor += speed;
                        if (originColor.a - aColor <= speed) a = false;
                    }
                    else
                    {
                        aColor -= speed;
                        if (aColor - originColor.a <= speed) a = false;
                    }
                }

                text.color = new Color(rColor, gColor, bColor, aColor);
                yield return waitTime;
            }
            text.color = originColor;
        }
        public static void FadeEffect(List<GameObject> _fadeObjList, bool isFade, float _duration, bool isReturn ,System.Action callback)
        {
            for (int i = 0; i < _fadeObjList.Count; i++)
            {
                Text text = _fadeObjList[i].GetComponent<Text>();
                Image image = _fadeObjList[i].GetComponent<Image>();
                if (text != null)
                {
                    text.DOKill();
                    text.color = isFade ? new Color(text.color.r, text.color.g, text.color.b, 1) : new Color(text.color.r, text.color.g, text.color.b, 0);
                    text.DOFade(isFade ? 0 : 1, _duration).OnComplete(() => {
                        if (isReturn)
                            text.color = isFade ? new Color(text.color.r, text.color.g, text.color.b, 1) : new Color(text.color.r, text.color.g, text.color.b, 0);
                    });
                }
                if (image != null)
                {
                    image.DOKill();
                    image.color = isFade ? new Color(image.color.r, image.color.g, image.color.b, 1) : new Color(image.color.r, image.color.g, image.color.b, 0);
                    image.DOFade(isFade ? 0 : 1, _duration).OnComplete(()=> {
                        if (isReturn)
                            image.color = isFade ? new Color(image.color.r, image.color.g, image.color.b, 1) : new Color(image.color.r, image.color.g, image.color.b, 0);
                    });
                }
            }

            DOTween.Sequence().InsertCallback(_duration, ()=> callback());
        }
    }

    public class BigIntegerManager
    {
        private static readonly BigInteger _unitSize = 1000;
        private static Dictionary<string, BigInteger> _unitsMap = new Dictionary<string, BigInteger>();
        private static Dictionary<string, int> _idxMap = new Dictionary<string, int>();
        private static readonly List<string> _units = new List<string>();
        private static int _unitCapacity = 5;
        private static readonly int _asciiA = 65;
        private static readonly int _asciiZ = 90;
        private static bool isInitialize = false;
        private static void UnitInitialize(int capacity)
        {
            _unitCapacity += capacity;

            //Initialize 0~999
            _units.Clear();
            _unitsMap.Clear();
            _idxMap.Clear();
            _units.Add("");
            _unitsMap.Add("", 0);
            _idxMap.Add("", 0);


            //capacity만큼 사전생성, capacity가 1인경우 A~Z
            //capacity가 2인경우 AA~AZ
            //capacity 1마다 ascii 알파벳 26개 생성되는 원리
            for (int n = 0; n <= _unitCapacity; n++)
            {
                for (int i = _asciiA; i <= _asciiZ; i++)
                {
                    string unit = null;
                    if (n == 0)
                        unit = ((char)i).ToString();
                    else
                    {
                        var nCount = (float)n / 26;
                        var nextChar = _asciiA + n - 1;
                        var fAscii = (char)nextChar;
                        var tAscii = (char)i;
                        unit = $"{fAscii}{tAscii}";
                    }
                    _units.Add(unit);
                    _unitsMap.Add(unit, BigInteger.Pow(_unitSize, _units.Count - 1));
                    _idxMap.Add(unit, _units.Count - 1);
                }
            }
            isInitialize = true;
        }


        private static int GetPoint(int value)
        {
            return (value % 1000) / 100;
        }

        private static (int value, int idx, int point) GetSize(BigInteger value)
        {
            //단위를 구하기 위한 값으로 복사
            var currentValue = value;
            var current = (value / _unitSize) % _unitSize;
            var idx = 0;
            var lastValue = 0;
            // 현재 값이 999(unitSize) 이상인경우 나눠야함.
            while (currentValue > _unitSize - 1)
            {
                var predCurrentValue = currentValue / _unitSize;
                if (predCurrentValue <= _unitSize - 1)
                    lastValue = (int)currentValue;
                currentValue = predCurrentValue;
                idx += 1;
            }

            var point = GetPoint(lastValue);
            var originalValue = currentValue * 1000;
            while (_units.Count <= idx)
                UnitInitialize(5);
            return ((int)currentValue, idx, point);
        }

        /// <summary>
        /// 숫자를 단위로 리턴
        /// </summary>
        /// <param name="value">값</param>
        /// <returns></returns>
        public static string GetUnit(BigInteger value)
        {
            if (isInitialize == false)
                UnitInitialize(5);

            var sizeStruct = GetSize(value);
            return $"{sizeStruct.value}.{sizeStruct.point}{_units[sizeStruct.idx]}";
        }
        public static string GetUnit(string _value)
        {
            BigInteger value = BigInteger.Parse(_value);
            if (isInitialize == false)
                UnitInitialize(5);

            var sizeStruct = GetSize(value);
            return $"{sizeStruct.value}.{sizeStruct.point}{_units[sizeStruct.idx]}";
        }

        /// <summary>
        /// 단위를 숫자로 변경
        /// 10A = 10000으로 리턴
        /// 1.2A = 1200으로 리턴
        /// 소수점 1자리만 지원함
        /// </summary>
        /// <param name="unit">단위</param>
        /// <returns></returns>
        public static BigInteger UnitToValue(string unit)
        {
            if (isInitialize == false)
                UnitInitialize(5);

            var split = unit.Split('.');
            //소수점에 관한 연산 들어감
            if (split.Length >= 2)
            {
                var value = BigInteger.Parse(split[0]);
                var point = BigInteger.Parse((Regex.Replace(split[1], "[^0-9]", "")));
                var unitStr = Regex.Replace(split[1], "[^A-Z]", "");

                if (point == 0) return (_unitsMap[unitStr] * value);
                else
                {
                    var unitValue = _unitsMap[unitStr];
                    return (unitValue * value) + (unitValue / 10) * point;
                }

            }
            //비소수 연산 들어감
            else
            {
                var value = BigInteger.Parse((Regex.Replace(unit, "[^0-9]", "")));
                var unitStr = Regex.Replace(unit, "[^A-Z]", "");
                while (_unitsMap.ContainsKey(unitStr) == false)
                    UnitInitialize(5);
                var result = _unitsMap[unitStr] * value;

                if (result == 0)
                    return int.Parse((unit));
                else
                    return result;
            }
        }



        public static int GetScore(string _value)
        {
            if (MyMath.CompareValue(_value, "0") != 1)
            {
                return 0;
            }

            int sizeLength = 5;
            int valueLength = 5;

            int size = _value.Length;

            string zeroTemp = "";
            for (int i = 0; i < sizeLength; i++)
                zeroTemp += "0";

            string stringSize = string.Format("{0:" + zeroTemp + "}", size);

            zeroTemp = "";
            for (int i = 0; i < valueLength; i++)
                zeroTemp += "0";

            int valueLen = valueLength;
            if (valueLen > size)
                valueLen = size;

            int value = int.Parse(_value.Substring(0, valueLen));
            string stringValue = string.Format("{0:" + zeroTemp + "}", value);

            string r = stringSize + stringValue;
            int result = int.Parse(stringSize + stringValue);

            return result;
        }

        public static string GetValue(int _score)
        {
            int sizeLength = 5;
            int valueLength = 5;
            string zeroTemp = "";
            for (int i = 0; i < valueLength + sizeLength; i++)
                zeroTemp += "0";

            string stringValue = string.Format("{0:" + zeroTemp + "}", _score);

            int size = int.Parse(stringValue.Substring(0, sizeLength));
            int value = int.Parse(stringValue.Substring(sizeLength, valueLength));

            string result = value.ToString().PadRight(size, '0');

            return result;
        }
    }

    public class GameInfo
    {
        // 전투력 계산 
        public static string GetPower()
        {
            string total = "0";

            return total;
        }
  
        public static bool IsCritical(int _criPer)
        {
            float r = Random.RandomRange(0f, 1f);
            return r < (_criPer / 1000f);
        }
    }

}