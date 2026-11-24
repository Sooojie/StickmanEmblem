using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatFormula : MonoBehaviour
{
	public Tilemap forestTilemap;
	public Tilemap mountainTilemap;
	public void InitiateCombat(Unit attacker, 
	Unit defender, 
	Vector3Int combatAtkCell,
	Vector3Int combatDefCell)
	{
		
	//Terrain bonuses for attacker, and Defender- in alternating Parts.
		int TABonus = 0;
		int TDBonus = 0;
		int TADBonus = 0;
		int TAABonus = 0;
		int TDDBonus = 0;
		int TDABonus = 0;
		//Weapon Effectiveness by Attacker
		int EA = 0;
		int ED = 0;
		if ((attacker.equippedWeapon.type == "Sword" && defender.equippedWeapon.type == "Mace") || 
		(attacker.equippedWeapon.type == "Mace" && defender.equippedWeapon.type == "Spear") || 
		(attacker.equippedWeapon.type == "Spear" && defender.equippedWeapon.type == "Sword"))
		{
			EA = 1;
			ED = -1;
		}
		//Weapon Effectiveness by Defender
		if ((attacker.equippedWeapon.type == "Sword" && attacker.equippedWeapon.type == "Mace") || 
		(defender.equippedWeapon.type == "Mace" && attacker.equippedWeapon.type == "Spear") || 
		(defender.equippedWeapon.type == "Spear" && attacker.equippedWeapon.type == "Sword"))
		{
			ED = 1;
			EA = -1;
		}

		int AAtk = CalcAttack(attacker, EA);
		int AHit = CalcHit(attacker, EA);
		int DAtk =  CalcAttack(attacker, ED);
		int DHit = CalcHit(defender, ED);
		int AattackSpeed = CalcAttackSpeed(attacker);
		int DattackSpeed = CalcAttackSpeed(defender);

		//Calculate Terrain Def Bonuses
		if (mountainTilemap != null &&
			mountainTilemap.HasTile(combatAtkCell))
		{
			TABonus = 1;
		}
		if (forestTilemap != null &&
			forestTilemap.HasTile(combatAtkCell))
		{
			TABonus = 2;
		}
		if (TABonus == 1 || TABonus == 2)
		{
			TDDBonus = 1; 
		}
		if (TABonus == 1)
		{
			TAABonus = 30;
		}
		if (TABonus == 2)
		{
			TAABonus = 20;
		}
		if (mountainTilemap != null &&
			mountainTilemap.HasTile(combatDefCell))
		{
			TDBonus = 1;
		}
		if (forestTilemap != null &&
			forestTilemap.HasTile(combatDefCell))
		{
			TDBonus = 2;
		}
		if (TDBonus == 1 || TDBonus == 2)
		{
			TDDBonus = 1; 
		}
		if (TDBonus == 1)
		{
			TDABonus = 30;
		}
		if (TDBonus == 2)
		{
			TDABonus = 20;
		}
		
		int ADef = CalcDef(attacker, TADBonus);
		int DDef = CalcDef(defender, TDDBonus);
		int ARes = CalcRes(attacker);
		int DRes = CalcRes(defender);
		int AAvo = CalcAvo(attacker, AattackSpeed, TAABonus);
		int DAvo = CalcAvo(defender, DattackSpeed, TDABonus);

		//Now to fill in the actual combat part here eventually tm
		Fight(attacker, defender, AAtk, AHit, DDef, DRes, DAvo);
		if (defender.unitStats.health > 0 && (attacker.equippedWeapon.type != "Bow" || attacker.equippedWeapon.type != "Staff"))
		{
			Fight(defender, attacker, DAtk, DHit, ADef, ARes, AAvo);
			if (AattackSpeed + 3 < DattackSpeed)
			{
				//double hit, defender.
				Fight(defender, attacker, DAtk, DHit, ADef, ARes, AAvo);
			}
		}
		if (AattackSpeed > DattackSpeed+3)
		{
			//Double Hit, attacker
			Fight(attacker, defender, AAtk, AHit, DDef, DRes, DAvo);
		}
	}

	public void Fight(Unit A, Unit B, int AAtk, int AHit, int DDef, int DRes, int DAvo)
	{	
		float HitAttempt = Random.Range(0.0F, 100.0F);
		float CritAttempt = Random.Range(0.0F, 100.0F);
		int Crit = 1;

		if ((A.equippedWeapon.type.crit + A.unitStats.skill/2) - (B.unitStats.luck) > CritAttempt)
		{
			Crit = 3;
		}
		if (HitAttempt < (float)(AHit-DAvo))
		{
			B.unitStats.health = B.unitStats.health - Crit*(AAtk - DDef);		
		}
	}

	private int displayHitrate(Unit attacker, 
	Unit defender, 
	Vector3Int combatAtkCell,
	Vector3Int combatDefCell)
	{
	//Terrain bonuses for attacker, and Defender- in alternating Parts.
		int TDBonus = 0;
		int TDABonus = 0;
	//Weapon Effectiveness by Attacker
		int EA = 0;
	//Currently setting away weapon methods until I figure out how it's equipped
	if ((attacker.equippedWeapon.type == "Sword" && defender.equippedWeapon.type == "Mace") || 
	(attacker.equippedWeapon.type == "Mace" && defender.equippedWeapon.type == "Spear") || 
	(attacker.equippedWeapon.type == "Spear" && defender.equippedWeapon.type == "Sword"))
		{
	EA = 1;
	}
	//Weapon Effectiveness by Defender
	if ((defender.equippedWeapon.type == "Sword" && attacker.equippedWeapon.type == "Mace") || 
	(defender.equippedWeapon.type == "Mace" && attacker.equippedWeapon.type == "Spear") || 
	(defender.equippedWeapon.type == "Spear" && attacker.equippedWeapon.type == "Sword"))
		{
	EA = -1;
	}
	//Calculate attack combat stats
		int AHit = CalcHit(attacker, EA);
		int DattackSpeed = CalcAttackSpeed(defender);
		//Calculate Terrain Def Bonuses
		if (mountainTilemap != null &&
			mountainTilemap.HasTile(combatDefCell))
		{
			TDBonus = 1;
		}
		if (forestTilemap != null &&
			forestTilemap.HasTile(combatDefCell))
		{
			TDBonus = 2;
		}
		if (TDBonus == 1)
		{
			TDABonus = 30;
		}
		if (TDBonus == 2)
		{
			TDABonus = 20;
		}
		int DAvo = CalcAvo(defender, DattackSpeed, TDABonus);
			return (AHit - DAvo);
	}

	private int CalcAttack(Unit unit, int E)
	{
		int Mult = 1;
		if (E == 1)
		{
		Mult = 3;
		}

		return unit.unitStats.attack + (Mult)*(unit.equippedWeapon.type.might+E);
	}

	private int CalcHit(Unit unit, int E)
	{
		return unit.equippedWeapon.type.hit + 2 * unit.unitStats.skill + (unit.unitStats.luck / 2) + (E * 10);
	}

	private int CalcAttackSpeed(Unit unit)
	{
		if (unit.unitStats.constitution > unit.equippedWeapon.type.weight)
		{
			if (unit.unitStats.speed > 0)
			{
				return unit.unitStats.speed;
			}
		}
		else
		{
			if (unit.unitStats.speed > 0)
			{
				return unit.unitStats.speed + unit.unitStats.constitution - unit.equippedWeapon.type.weight;
			}
		}
	return 0;
	}

	private int CalcDef(Unit unit, int terrainBonus)
	{
		return unit.unitStats.defense + terrainBonus;
	}

	private int CalcRes(Unit unit)
	{
		return unit.unitStats.resistance;
	}

	private int CalcAvo(Unit unit, int AS, int terrainBonus)
	{
	return AS * 2 + unit.unitStats.luck + terrainBonus;
	}
}