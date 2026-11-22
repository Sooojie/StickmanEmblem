using System.Data;
using System.Runtime.CompilerServices;

void InitiateCombat(Unit attacker, 
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
if ((attacker.weapon == "Sword" && defender.weapon == "Mace") || 
(attacker.weapon == "Mace" && defender.weapon == "Spear") || 
(attacker.weapon == "Spear" && defender.weapon == "Sword"))
	{
EA = 1;
ED = -1;
}
//Weapon Effectiveness by Defender
if ((defender.weapon == "Sword" && attacker.weapon == "Mace") || 
(defender.weapon == "Mace" && attacker.weapon == "Spear") || 
(defender.weapon == "Spear" && attacker.weapon == "Sword"))
	{
ED = 1;
EA = -1;
}
//Calculate attack combat stats
int AAtk = CalcAttack(attacker, EA);
int AHit = CalcHit(attacker, EA);
int DAtk =  CalcAttack(attacker, ED);
int DHit = CalcHit(defender, ED);
int AattackSpeed = CalcAttackSpeed(attacker);
int DattackSpeed = CalcAttackSpeed(defender);
//Calculate Terrain Def Bonuses
if (mountainTileMap != null &&
	mountainTileMap.hasTile(combatAtkCell))
{
TABonus = 1;
}
if (forestTileMap != null &&
	forestTileMap.hasTile(combatAtkCell))
{
TABonus = 2;
}
if (TABonus == 1 || TABonus == 2){
TDDBonus = 1; }
if (TABonus == 1)
{
TAABonus = 30;
}
if (TABonus == 2)
{
TAABonus = 20;
}
if (mountainTileMap != null &&
	mountainTileMap.hasTile(combatDefCell))
{
TDBonus = 1;
}
if (forestTileMap != null &&
	forestTileMap.hasTile(combatDefCell))
{
TDBonus = 2;
}
if (TDBonus == 1 || TDBonus == 2)
{
TDDBonus = 1; }
if (TDBonus == 1)
{
TDABonus = 30;
}
if (TDBonus == 2){
TDABonus = 20;
}
int ADef = CalcDef(attacker, TADBonus);
int DDef = CalcDef(defender, TDDBonus);
int ARes = CalcRes(attacker);
int DRes = CalcRes(defender);
int AAvo = CalcAvo(attacker, AattackSpeed, TAABonus);
int DAvo = CalcAvo(defender, DattackSpeed, TDABonus);

//Now to fill in the actual combat part here eventually tm
	Combat(attacker, defender, AAtk, AHit, DDef, DRes, DAvo);
	if (defender.health > 0 && (attacker.weapon != "Bow" || attacker.weapon != "Staff"))
	{
		Combat(defender, attacker, DAtk, DHit, ADef, ARes, AAvo);
		if (AattackSpeed + 3 < Dattackspeed)
		{
			//double hit, defender.
			Combat(defender, attacker, DAtk, DHit, ADef, ARes, AAvo);
		}
	}
	if (Aattackspeed > DattackSpeed+3)
	{
		//Double Hit, attacker
		Combat(attacker, defender, AAtk, AHit, DDef, DRes, DAvo);
	}
}

void Combat(Unit A, Unit B, int AAtk, int AHit, int DDef, int DRes, int DAvo)
{	float HitAttempt = Random.Range(0.0F, 100.0F);
	float CritAttempt = Random.Range(0.0F, 100.0F);
	int Crit = 1;
	if ((A.weapon.crit + A.skill/2) - (B.luck) > CritAttempt)
	{
		Crit = 3;
	}
	if (HitAttempt < (float)(AHit-DAvo))
	{
		if (A.weapon == "Staff")
		{
		B.health = B.health - Crit*(AAtk - DRes);		
		}
		else
		{
			B.health = B.health - Crit*(AAtk - DDef);		
		}
	}
}

int displayHitrate(Unit attacker, 
Unit defender, 
Vector3Int combatAtkCell,
Vector3Int combatDefCell)
{
//Terrain bonuses for attacker, and Defender- in alternating Parts.
	int TDBonus = 0;
int TDABonus = 0;
//Weapon Effectiveness by Attacker
int EA = 0;
if ((attacker.weapon == "Sword" && defender.weapon == "Mace") || 
(attacker.weapon == "Mace" && defender.weapon == "Spear") || 
(attacker.weapon == "Spear" && defender.weapon == "Sword"))
	{
EA = 1;
}
//Weapon Effectiveness by Defender
if ((defender.weapon == "Sword" && attacker.weapon == "Mace") || 
(defender.weapon == "Mace" && attacker.weapon == "Spear") || 
(defender.weapon == "Spear" && attacker.weapon == "Sword"))
	{
EA = -1;
}
//Calculate attack combat stats
int AHit = CalcHit(attacker, EA);
int DattackSpeed = CalcAttackSpeed(defender);
//Calculate Terrain Def Bonuses
if (mountainTileMap != null &&
	mountainTileMap.hasTile(combatDefCell))
{
TDBonus = 1;
}
if (forestTileMap != null &&
	forestTileMap.hasTile(combatDefCell))
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

int CalcAttack(Unit unit, int E)
{
	int Mult = 1;
	if (E == 1)
	{
	Mult = 3;
	}
	return unit.attack + (Mult)*(unit.weapon.might+E);
}

int CalcHit(Unit unit, int E)
{
	return unit.weapon.hit + 2 * unit.skill + (unit.luck / 2) + (E * 10);
}

int CalcAttackSpeed(Unit unit)
{
	if (unit.constitution > unit.weapon.weight)
	{
		if (unit.speed > 0)
{
			return unit.speed;
		}
	}
	else
	{
		if (unit.speed > 0)
{
			return unit.speed + unit.constitution - unit.weapon.weight;
		}
	}
return 0;
}

int CalcDef(Unit unit, int terrainBonus)
{
	return unit.defense + terrainBonus;
}

int CalcRes(Unit unit)
{
	return unit.resistance;
}

int CalcAvo(Unit unit, int AS, int terrainBonus)
{
return AS * 2 + unit.luck + terrainBonus;
}
