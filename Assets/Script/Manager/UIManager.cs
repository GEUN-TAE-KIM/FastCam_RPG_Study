using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public static UIManager aInstance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new UIManager();
            }
            return sInstance;
        }
    }

    public delegate void OnShowHUDText(bool InIsShow, string Text);
    public OnShowHUDText aOnShowHUDText { get; set; }

    public delegate void OnSetExp(int InExp, int InMaxExp);
    public OnSetExp aOnSetExp { get; set; }

    public delegate void OnShowLevelUpStateUI(bool InIsShow);
    public OnShowLevelUpStateUI aOnShowLevelUpStateUI { get; set; }

    public delegate void OnLevelUp(List<SkillLevelData> InNextLevelSkillDatas);
    public OnLevelUp aOnLevelUp { get; set; }

    public void Clear()
    {
        GameHudComp = null;
    }

    public void ShowHUDText(string InText)
    {
        if (aOnShowHUDText != null)
        {
            aOnShowHUDText(true, InText);
        }
    }
    public void HideHUDText()
    {
        if (aOnShowHUDText != null)
        {
            aOnShowHUDText(false, string.Empty);
        }
    }

    public void ShowLevelupStateUI(bool InIsShow)
    {
        if (aOnShowLevelUpStateUI != null)
        {
            aOnShowLevelUpStateUI(InIsShow);
        }
    }

    public void SetExp(int InCurrentExp, int InMaxExp)
    {
        if (aOnSetExp != null)
        {
            aOnSetExp(InCurrentExp, InMaxExp);
        }
    }

    public void MyUnitLevelUp(int InLevel, List<SkillLevelData> InNextLevelSkillDatas)
    {
        if (aOnLevelUp != null)
        {
            aOnLevelUp(InNextLevelSkillDatas);
        }
    }

    public void SetManualSkills(List<ActiveSkillData> InSkillDatas)
    {
        if (GameHudComp == null)
        {
            return;
        }
        if (GameHudComp.SkillUIComp != null)
        {
            GameHudComp.SkillUIComp.UpdateManualSkills(InSkillDatas);
        }
    }

    public void SetHUDComponent(HUDComponent InHudComp)
    {
        GameHudComp = InHudComp;
    }

    private static UIManager sInstance = null;
    private HUDComponent GameHudComp;
}
