using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EBToolkit.Gui.Controls
{
	/// <summary>
	/// A <see cref="NumericUpDown"/> control which is limited in range to the
	/// range of a <see cref="byte"/>
	/// </summary>
	/// <seealso cref="byte"/>
	/// <seealso cref="NumericUpDown"/>
	/// <seealso cref="NumericUpDownUInt16"/>
	/// <seealso cref="NumericUpDownUInt32"/>
	public class NumericUpDownByte : NumericUpDown
	{
		/// <summary>
		/// The field for the minimum value of this control
		/// </summary>
		private byte _minimum = Byte.MinValue;

		/// <summary>
		/// The field for the maximum value of this control
		/// </summary>
		private byte _maximum = Byte.MaxValue;

		/// <summary>
		/// The field for the value of this control
		/// </summary>
		private byte _value;

		/// <summary>
		/// The amount of decimal places. Always 0.
		/// </summary>
		public new int DecimalPlaces
		{
			get { return 0; }
		}

		/// <summary>
		/// The minimum value for this control. Defaults to <see cref="Byte.MinValue"/>
		/// </summary>
		/// <seealso cref="Byte.MinValue"/>
		/// <seealso cref="Maximum"/>
		/// <seealso cref="Value"/>
		[DefaultValue(Byte.MinValue)]
		public new byte Minimum
		{
			get { return this._minimum; }
			set
			{
				this._minimum = value;
				base.Minimum = this._minimum;
			}
		}

		/// <summary>
		/// The maximum value for this control. Defaults to <see cref="Byte.MaxValue"/>
		/// </summary>
		/// <seealso cref="Byte.MaxValue"/>
		/// <seealso cref="Minimum"/>
		/// <seealso cref="Value"/>
		[DefaultValue(Byte.MaxValue)]
		public new byte Maximum
		{
			get { return this._maximum; }
			set
			{
				this._maximum = value;
				base.Maximum = this._maximum;
			}
		}

		/// <summary>
		/// The value of this control. Defaults to <see cref="Byte.MinValue"/>
		/// </summary>
		/// <seealso cref="Byte.MinValue"/>
		/// <seealso cref="Minimum"/>
		/// <seealso cref="Maximum"/>
		[DefaultValue(Byte.MinValue)]
		public new byte Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				base.Value = this._value;
			}
		}

		/// <summary>
		/// Creates a new instance of a <see cref="NumericUpDownByte"/>
		/// </summary>
		public NumericUpDownByte()
		{
			this.Minimum = Byte.MinValue;
			this.Maximum = Byte.MaxValue;
			this.Value = this.Minimum;
			base.ValueChanged += new EventHandler(this.SyncValue);
		}

		/// <summary>
		/// A private helper method to sync the value of this control with
		/// the value of the underlying <see cref="NumericUpDown.Value"/>
		/// </summary>
		/// <param name="sender">Sender info. Ignored.</param>
		/// <param name="args">EventArgs. Ignored.</param>
		private void SyncValue(object sender, EventArgs args)
		{
			this._value = (byte)base.Value;
		}
	}
}
