using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ItemSO_0", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string code;
    public string nameCode;
    [PreviewField (100)] public Sprite sprite;

    public ItemGrade itemGrade;
    public ItemType itemType;

    #region 정보

    [LabelText("이동속도% 증가")] [ShowIf("itemType" , ItemType.부스터)] [MinValue(0)] public int boosterCount;
    [LabelText("+ 이동속도% 증가")] [ShowIf("itemType", ItemType.부스터)] [MinValue(0)] public int boosterCount_levelup;

    [LabelText("발사체공격력 증가")] [ShowIf("itemType", ItemType.탄환)] [MinValue(0)] public int bulletCount;
    [LabelText("+ 발사체공격력 증가")] [ShowIf("itemType", ItemType.탄환)] [MinValue(0)] public int bulletCount_levelup;

    [LabelText("공격속도 증가")] [ShowIf("itemType", ItemType.재장전)] [MinValue(0)] public int reloadCount;
    [LabelText("+ 공격속도 증가")] [ShowIf("itemType", ItemType.재장전)] [MinValue(0)] public int reloadCount_levelup;

    [LabelText("최대HP 증가")] [ShowIf("itemType", ItemType.에너지)] [MinValue(0)] public int energyCount;
    [LabelText("+ 최대HP 증가")] [ShowIf("itemType", ItemType.에너지)] [MinValue(0)] public int energyCount_levelup;

    [LabelText("코어 드랍률 증가 (최대1000)")] [ShowIf("itemType", ItemType.코어드랍)] [MinValue(0)] [MaxValue(1000)]  public int coredropCount;
    [LabelText("+ 코어 드랍률 증가 (최대1000)")] [ShowIf("itemType", ItemType.코어드랍)] [MinValue(0)] [MaxValue(1000)] public int coredropCount_levelup;

    [LabelText("게임 시작시 HP회복 (최대1000)")] [ShowIf("itemType", ItemType.에너지드링크)] [MinValue(0)] [MaxValue(1000)] public int energyDringCount;
    [LabelText("+ 게임 시작시 HP회복 (최대1000)")] [ShowIf("itemType", ItemType.에너지드링크)] [MinValue(0)] [MaxValue(1000)] public int energyDringCount_levelup;

    [LabelText("방어력 증가")] [ShowIf("itemType", ItemType.아머)] [MinValue(0)] public int armorCount;
    [LabelText("+ 방어력 증가")] [ShowIf("itemType", ItemType.아머)] [MinValue(0)] public int armorCount_levelup;

    [LabelText("치명타확률 증가 (최대1000)")] [ShowIf("itemType", ItemType.센서)] [MinValue(0)] [MaxValue(1000)] public int senserCount;
    [LabelText("+ 치명타확률 증가 (최대1000)")] [ShowIf("itemType", ItemType.센서)] [MinValue(0)] [MaxValue(1000)] public int senserCount_levelup;

    [LabelText("발사체 내구도 증가")] [ShowIf("itemType", ItemType.탄환저항)] [MinValue(0)] public int bulletStabilityCount;
    [LabelText("+ 발사체 내구도 증가")] [ShowIf("itemType", ItemType.탄환저항)] [MinValue(0)] public int bulletStabilityCount_levelup;

    [LabelText("발사체 크기 증가 (최대1000)")] [ShowIf("itemType", ItemType.쇠구슬)] [MinValue(0)] [MaxValue(1000)] public int ironballCount;
    [LabelText("+ 발사체 크기 증가 (최대1000)")] [ShowIf("itemType", ItemType.쇠구슬)] [MinValue(0)] [MaxValue(1000)] public int ironballCount_levelup;

    [LabelText("피해를 입은 후 무적시간 증가")] [ShowIf("itemType", ItemType.열처리)] [MinValue(0)] public int heatTreatmentCount;
    [LabelText("+ 피해를 입은 후 무적시간 증가")] [ShowIf("itemType", ItemType.열처리)] [MinValue(0)] public int heatTreatmentCount_levelup;

    [LabelText("적 수에 비례하여 체력회복 (최대1000)")] [ShowIf("itemType", ItemType.충전)] [MinValue(0)] [MaxValue(1000)] public int chargeCount;
    [LabelText("+ 적 수에 비례하여 체력회복 (최대1000)")] [ShowIf("itemType", ItemType.충전)] [MinValue(0)] [MaxValue(1000)] public int chargeCount_levelup;

    [LabelText("체력 흡수 (최대1000)")] [ShowIf("itemType", ItemType.에너지흡수)] [MinValue(0)] [MaxValue(1000)] public int energeAbsorptionCount;
    [LabelText("+ 체력 흡수 (최대1000)")] [ShowIf("itemType", ItemType.에너지흡수)] [MinValue(0)] [MaxValue(1000)] public int energeAbsorptionCount_levelup;

    [LabelText("희귀 아이템 나올 확률 증가 (최대1000)")] [ShowIf("itemType", ItemType.주사위)] [MinValue(0)] [MaxValue(1000)] public int diceCount;
    [LabelText("+ 희귀 아이템 나올 확률 증가 (최대1000)")] [ShowIf("itemType", ItemType.주사위)] [MinValue(0)] [MaxValue(1000)] public int diceCount_levelup;

    [LabelText("체력50%이하일때 공격력 증가")] [ShowIf("itemType", ItemType.과열)] [MinValue(0)] public int overHeatCount;
    [LabelText("+ 체력50%이하일때 공격력 증가")] [ShowIf("itemType", ItemType.과열)] [MinValue(0)] public int overHeatCount_levelup;

    [LabelText("인공위성 레이저 기본 공격력")] [ShowIf("itemType", ItemType.인공위성)] [MinValue(0)] public int satelliteAtkCount;
    [LabelText("인공위성 레이저 기본 공격속도")] [ShowIf("itemType", ItemType.인공위성)] [MinValue(0)] public float satelliteAtkspeedCount;
    [LabelText("+ 인공위성 레이저 공격력")] [ShowIf("itemType", ItemType.인공위성)] [MinValue(0)] public int satelliteAtkCount_levelup;
    [LabelText("+ 인공위성 레이저 공격속도")] [ShowIf("itemType", ItemType.인공위성)] [MaxValue(0)] public float satelliteAtkspeedCount_levelup;

    [LabelText("치명 데미지 증가 (최대1000)")] [ShowIf("itemType", ItemType.정밀사격)] [MinValue(0)] [MaxValue(1000)] public int precisionCount;
    [LabelText("+ 치명 데미지 증가 (최대1000)")] [ShowIf("itemType", ItemType.정밀사격)] [MinValue(0)] [MaxValue(1000)] public int precisionCount_levelup;

    [LabelText("크기 축소 (최대1000)")] [ShowIf("itemType", ItemType.축소장치)] [MinValue(0)] [MaxValue(1000)] public int smallCount;
    [LabelText("+ 크기 축소 (최대1000)")] [ShowIf("itemType", ItemType.축소장치)] [MinValue(0)] [MaxValue(1000)] public int smallCount_levelup;

    [LabelText("전기 발사체 기본 공격력")] [ShowIf("itemType", ItemType.전기폭발)] [MinValue(0)] public int electricExplosionCount;
    [LabelText("+ 전기 발사체 공격력 증가")] [ShowIf("itemType", ItemType.전기폭발)] [MinValue(0)] public int electricExplosionCount_levelup;

    [LabelText("방어막 생성 시간")] [ShowIf("itemType", ItemType.방어막)] [MinValue(1)] public float barrierTime;

    [LabelText("가만히 있을때 공격속도 증가")] [ShowIf("itemType", ItemType.머신건)] [MinValue(0)] public int machineGun;
    [LabelText("+ 가만히 있을때 공격속도 증가")] [ShowIf("itemType", ItemType.머신건)] [MinValue(0)] public int machineGun_levelup;

    [LabelText("아이템 복사 확률")] [ShowIf("itemType", ItemType.복사기)] [MinValue(0)] [MaxValue(1000)] public int copy;

    [LabelText("적에게 피해를 주면 손상피해")] [ShowIf("itemType", ItemType.혼합기름)] [MinValue(0)] public int mixedOil;
    [LabelText("+ 적에게 피해를 주면 손상피해")] [ShowIf("itemType", ItemType.혼합기름)] [MinValue(0)] public int mixedOil_levelup;

    [LabelText("회피확률")] [ShowIf("itemType", ItemType.니트로)] [MinValue(0)] [MaxValue(1000)] public int nitro;

    [LabelText("발사체 공격력 증가")] [ShowIf("itemType", ItemType.매그넘44)] [MinValue(0)] public int magnum;

    [LabelText("최대 HP증가%")] [ShowIf("itemType", ItemType.수퍼로봇)] [MinValue(0)] [MaxValue(1000)] public float superRobot_maxhp;

    [LabelText("HP흡수")] [ShowIf("itemType", ItemType.스펀지)] [MinValue(0)] [MaxValue(1000)] public int sponge;

    [LabelText("영웅 아이템 출현확률")] [ShowIf("itemType", ItemType.복주머니)] [MinValue(0)] [MaxValue(1000)] public int luckyBag;

    [LabelText("회피율 증가")] [ShowIf("itemType", ItemType.포켓봇)] [MinValue(0)] [MaxValue(1000)] public int pokebot;

    [LabelText("탐색 포인트 증가")] [ShowIf("itemType", ItemType.오토캐드)] [MinValue(0)] public int autoCAD;

    [LabelText("멈춰 있을 때 점점 방어력 증가")] [ShowIf("itemType", ItemType.벙커)] [MinValue(0)] public int bunker;

    [LabelText("공격속도 증가")] [ShowIf("itemType", ItemType.직선탄)] [MinValue(0)] public int staightShot;

    [LabelText("적 탄환 감속")] [ShowIf("itemType", ItemType.냉기)] [MinValue(0)] public int coldShot;

    [LabelText("공격력 증가")] [ShowIf("itemType", ItemType.다발탄)] [MinValue(0)] public int multipleShot;

    [LabelText("회복 수치")] [ShowIf("itemType", ItemType.침착)] [MinValue(0)] public int composure;
    #endregion

    [LabelText("아이템 설명")] public string scriptCode;

    [LabelText("최대 레벨")] [MinValue(1)] public int maxLevel;

    [LabelText("추가 아이템 발생 레벨조건")] [MinValue(1)] public int plusItemLevel;
    [LabelText("추가 아이템 발생 SO")] public ItemSO plusItem;
}

public enum ItemType
{
    부스터,
    탄환,
    재장전,
    에너지,
    코어드랍,
    에너지드링크,
    아머,
    센서,
    탄환저항,
    쇠구슬,
    열처리,
    충전,
    에너지흡수,
    주사위,
    과열,
    인공위성,
    정밀사격,
    축소장치,
    전기폭발,
    드론,
    설계도,
    방어막,
    머신건,
    복사기,
    혼합기름,
    니트로,
    매그넘44,
    수퍼로봇,
    수집가,
    레드불,
    가시갑옷,
    레이저,
    자이로스코프,
    팩맨,
    와이퍼,
    침착,
    전기장,
    스펀지,
    복주머니,
    재가동,
    스타링크,
    하이눈,
    포켓봇,
    일렉트로,
    멀티곱터,
    오토캐드,
    반사장치,
    벙커,
    자동생성,
    스포일러,
    직선탄,
    냉기,
    다발탄,
    직선탄_강화,
    냉기_강화,
    다발탄_강화
}
public enum ItemGrade
{
    일반,
    희귀,
    영웅
}
