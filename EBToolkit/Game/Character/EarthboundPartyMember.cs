using EBToolkit.Game.Inventory;
using EBToolkit.Game.Text;
using System;
using System.IO;

namespace EBToolkit.Game.Character
{
	/// <summary>
	/// Represents a party member in the game EarthBound. Party members have two
	/// extra stats that other <see cref="EarthboundCharacter"/>s do not.
	/// </summary>
	public class EarthboundPartyMember : EarthboundCharacter, EarthboundSaveable
	{
		/// <summary>
		/// The value that <see cref="Vitality"/> will multiply to for a the new
		/// <see cref="EarthboundCharacter.HP"/> on a new level.
		/// <seealso cref="EarthboundCharacter.HP"/>
		/// <seealso cref="Vitality"/>
		/// <seealso cref="GetEstimatedMaxHPOnNewLevel"/>
		/// </summary>
		public const ushort VitalityMultiplier = 15;
		
		/// <summary>
		/// A value which controls what the new <see cref="EarthboundCharacter.HP"/> 
		/// max value will be on a new level.
		/// <seealso cref="IQ"/>
		/// <seealso cref="EarthboundCharacter.HP"/>
		/// <seealso cref="VitalityMultiplier"/>
		/// <seealso cref="GetEstimatedMaxHPOnNewLevel"/>
		/// </summary>
		public EquipmentChangeableStat Vitality;
		
		/// <summary>
		/// A value which controls what the new <see cref="EarthboundCharacter.PP"/>
		/// max value will be on a new level (or for a party member without PP, when
		/// they can fix items).
		/// <seealso cref="Vitality"/>
		/// <seealso cref="EarthboundCharacter.PP"/>
		/// <seealso cref="PsychicPointsState"/>
		/// <seealso cref="GetEstimatedMaxHPOnNewLevel"/>
		/// </summary>
		public EquipmentChangeableStat IQ;

		/// <summary>
		/// The inventory for this player.
		/// </summary>
		public PlayerInventory Inventory;

		/// <summary>
		/// Gets an estimated <see cref="RollingStat.MaxValue">max value</see> for 
		/// <see cref="EarthboundCharacter.HP"/> on the next level. This is not
		/// accurate when <see cref="Vitality"/> remains unchanged from the previous
		/// level, in which case a random number between 1 and 3 is used instead.
		/// </summary>
		/// <returns><see cref="Vitality"/> multipled by <see cref="VitalityMultiplier"/></returns>
		/// <seealso cref="RollingStat.MaxValue"/>
		/// <seealso cref="EarthboundCharacter.HP"/>
		/// <seealso cref="Vitality"/>
		/// <seealso cref="VitalityMultiplier"/>
		/// <seealso cref="GetEstimatedMaxPPOnNewLevel(PsychicPointsState)"/>
		/// <remarks>
		/// This function is adapted from the equation on the <a href="https://starmen.net/mother2/gameinfo/technical/equations.php">Starmen.net equations page</a>.
		/// </remarks>
		public ushort GetEstimatedMaxHPOnNewLevel()
		{
			return this.Vitality * VitalityMultiplier;
		}

		/// <summary>
		/// Gets an estimated <see cref="RollingStat.MaxValue">max value</see> for
		/// <see cref="EarthboundCharacter.PP"/> on the next level. As with
		/// <see cref="GetEstimatedMaxHPOnNewLevel"/>, this is not accurate when
		/// <see cref="IQ"/> remains unchanged from the previous level, in which
		/// case a random number between 1 and 3 will be used instead. This value
		/// may be different between different characters, as some characters get
		/// a larger boost in <see cref="EarthboundCharacter.PP"/> after a certain
		/// event or may not have any PP at all.
		/// </summary>
		/// <param name="PsychicPointsState">This character's psychic point state,
		/// which is used in determining how much the estimated max <see cref="EarthboundCharacter.PP"/>
		/// will be.</param>
		/// <returns><see cref="IQ"/> multiplied by the numeric value of <see cref="PsychicPointsState"/></returns>
		/// <seealso cref="RollingStat.MaxValue"/>
		/// <seealso cref="EarthboundCharacter.PP"/>
		/// <seealso cref="IQ"/>
		/// <seealso cref="PsychicPointsState"/>
		/// <seealso cref="GetEstimatedMaxHPOnNewLevel"/>
		/// <remarks>
		/// This function is adapted from the equation on the <a href="https://starmen.net/mother2/gameinfo/technical/equations.php">Starmen.net equations page</a>.
		/// </remarks>
		public ushort GetEstimatedMaxPPOnNewLevel(PsychicPointsState PsychicPointsState)
		{
			return this.IQ * (ushort)PsychicPointsState;
		}

		/// <inheritdoc/>
		public void WriteDataToStream(BinaryWriter writer)
		{
			EarthboundPlainTextEncoding PlainTextEncoding = new EarthboundPlainTextEncoding();
			writer.Write(PlainTextEncoding.GetBytesPadded(Name, 5));
			writer.Write(Level);
			writer.Write(Experience);
			writer.Write(HP.MaxValue);
			writer.Write(PP.MaxValue);
			writer.Write((byte)PermanentStatusEffect);
			writer.Write((byte)PossessionStatus);
			writer.Write((byte)BattleStatusEffect);
			writer.Write(FeelingStrange);
			writer.Write(CantConcentrateTurns);
			writer.Write(Homesick);
			//TODO: Shield
			Offense.WriteDataToStream(writer);
			Defense.WriteDataToStream(writer);
			Speed.WriteDataToStream(writer);
			Guts.WriteDataToStream(writer);
			Luck.WriteDataToStream(writer);
			Vitality.WriteDataToStream(writer);
			IQ.WriteDataToStream(writer);
			Inventory.WriteDataToStream(writer);
			HP.WriteDataToStream(writer);
			PP.WriteDataToStream(writer);
			throw new NotImplementedException("Weaknesses, miss rates, permanent boosts, shields, etc");
		}

		/// <summary>
		/// An enum value representing the different states that the increases in
		/// <see cref="RollingStat.MaxValue">max</see> <see cref="EarthboundCharacter.PP"/>
		/// can occur in EarthBound.
		/// </summary>
		/// <seealso cref="EarthboundCharacter.PP"/>
		/// <seealso cref="RollingStat.MaxValue"/>
		/// <seealso cref="IQ"/>
		/// <seealso cref="GetEstimatedMaxPPOnNewLevel(PsychicPointsState)"/>
		public enum PsychicPointsState : ushort
		{
			/// <summary>
			/// This user has no psychic abilities, so on level up, no psychic points
			/// will be granted.
			/// </summary>
			/// <remarks>
			/// In EarthBound, the character <a href="https://wikibound.info/wiki/Jeff">Jeff</a>
			/// would use this state, as he has no PSI abilities.
			/// </remarks>
			NoPsychicPoints = 0,
			/// <summary>
			/// Characters with PSI abilities normally level up their max PP to
			/// this level.
			/// </summary>
			Normal = 5,
			/// <summary>
			/// <a href="https://wikibound.info/wiki/Ness">Ness</a>, after completing
			/// the <a href="https://wikibound.info/wiki/Magicant">Magicant</a> area
			/// of the game will instead have his <see cref="EarthboundCharacter.PP"/>
			/// increase to this value multiplied by <see cref="Vitality"/> instead of
			/// the value of <see cref="Normal"/>
			/// </summary>
			NessPostMagicant = 10
		}
	}
}
