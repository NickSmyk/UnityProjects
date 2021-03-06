using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;


public static class EnumMethods {
	public static string GetDescription(System.Enum value) {
		var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
		var descriptionAttribute =
			enumMember == null
				? default(DescriptionAttribute)
				: enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
		return
			descriptionAttribute == null
				? value.ToString()
				: descriptionAttribute.Description;
	}
}

public enum Stat {
	AttackSpeed,
	AttackDamage,
	Life
}

public enum Notifications {
	[Description("Level Up")]
	LevelUp,
	[Description("Healed")]
	Healing,
	[Description("Attack Damege Increased")]
	IncreasedAttackDamage,
	[Description("Attack Speed Increased")]
	IncreasedAttackSpeed,
	[Description("Maximum Life Increased")]
	IncreasedMaximumLife,
	[Description("Enemies now are stronger")]
	EnemiesAreStronger
}

public enum MainCharacterAnimations {
	[Description("IdleAnimationMainCharacter")]
	MainCharacterIdle,
	[Description("RunningAnimationMainCharacter")]
	MainCharacterRun,
	[Description("AttackAnimationMainCharacter")]
	MainCharacterAttack,
	[Description("DeathAnimationMainCharacter")]
	MainCharacterDeath,

}
