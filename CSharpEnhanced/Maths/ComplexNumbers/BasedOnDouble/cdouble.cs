using System;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Extension of double to complex set of numbers. It's expressed as a+bi, where a is the real part, b is the
	/// imaginary part and i is the imaginary unit
	/// </summary>
	public class cdouble
    {
		#region cdoubleCore definition

		/// <summary>
		/// Core struct for complex numbers based on doubles. Encapsulates all fields which prevents desynchronization between
		/// cartesian and polar coordinates (ex: when real part is changed but the modulus and phase aren't adjusted)
		/// </summary>
		private class cdoubleCore
		{
			#region Private Members

			/// <summary>
			/// Real part of this number (cartesian coordinate). Backing store for <see cref="Re"/>
			/// </summary>
			private double mRe;

			/// <summary>
			/// Imaginary part of this number (cartesian coordinate). Backing store for <see cref="Im"/>
			/// </summary>
			private double mIm;

			/// <summary>
			/// Modulus of this number (polar coordinate). Backing store for <see cref="Mod"/>
			/// </summary>
			private double mMod;

			/// <summary>
			/// Phase of this number, in radians (polar coordinate). Backing store for <see cref="Phase"/>
			/// </summary>
			private double mPhase;

			#endregion

			#region Public Properties

			/// <summary>
			/// Real part of this number (cartesian coordinate)
			/// </summary>
			public double Re
			{
				get => mRe;
				set
				{
					if (mRe != value)
					{
						mRe = value;
						RecalculatePolarCoordinates();
					}
				}
			}

			/// <summary>
			/// Imaginary part of this number (cartesian coordinate)
			/// </summary>
			public double Im
			{
				get => mIm;
				set
				{
					if (mIm != value)
					{
						mIm = value;
						RecalculatePolarCoordinates();
					}
				}
			}

			/// <summary>
			/// Modulus of this number (polar coordinate)
			/// </summary>
			public double Mod
			{
				get => mMod;
				set
				{
					if (mMod != value)
					{
						mMod = value;
						RecalculateCartesianCoordinates();
					}
				}
			}


			/// <summary>
			/// Phase of this number, in radians (polar coordinate)
			/// </summary>
			public double Phase
			{
				get => mPhase;
				set
				{
					value = Helpers.ReduceAngle(value);

					if (mPhase != value)
					{
						mPhase = value;
						RecalculateCartesianCoordinates();
					}
				}
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Recalculates polar coordinates based on cartesian coordinates
			/// </summary>
			private void RecalculatePolarCoordinates()
			{
				mMod = Math.Sqrt(Math.Pow(Re, 2) + Math.Pow(mIm, 2));

				mPhase = Math.Atan2(Im, Re);
			}

			/// <summary>
			/// Recalculates cartesian coordinates based on polar coordinates
			/// </summary>
			private void RecalculateCartesianCoordinates()
			{
				mRe = mMod * Math.Cos(mPhase);

				mIm = mMod * Math.Sin(mPhase);
			}

			#endregion
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Core of this cdouble
		/// </summary>
		private readonly cdoubleCore mCore = new cdoubleCore();

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor for a 0-valued complex number
		/// </summary>
		public cdouble() { }

		/// <summary>
		/// Copy constructor, assigns the same values as in c
		/// </summary>
		/// <param name="c"></param>
		public cdouble(cdouble c)
		{
			Re = c.Re;
			Im = c.Im;
		}

		/// <summary>
		/// Constructor with 1 parameter for real part, imaginary part is set to 0
		/// </summary>
		/// <param name="re"></param>
		public cdouble(double re)
		{
			Re = re;
		}

		/// <summary>
		/// Constructor with 1 parameter for real part, imaginary part is set to 0
		/// </summary>
		/// <param name="re"></param>
		public cdouble(float re)
		{
			Re = re;
		}

		/// <summary>
		/// Constructor with 1 parameter for real part, imaginary part is set to 0
		/// </summary>
		/// <param name="re"></param>
		public cdouble(int re)
		{
			Re = re;
		}

		/// <summary>
		/// Constructor with 1 parameter for real part, imaginary part is set to 0
		/// </summary>
		/// <param name="re"></param>
		public cdouble(long re)
		{
			Re = re;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(double re, double im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(double re, float im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(double re, int im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(double re, long im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(float re, double im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(int re, double im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(long re, double im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(float re, float im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(float re, int im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(float re, long im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(int re, float im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(long re, float im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(int re, int im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(int re, long im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(long re, int im)
		{
			Re = re;
			Im = im;
		}

		/// <summary>
		/// Constructor with 2 parameters
		/// </summary>
		/// <param name="re">Real part of the number</param>
		/// <param name="im">Imaginary part of the number</param>
		public cdouble(long re, long im)
		{
			Re = re;
			Im = im;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Real part of this complex number (cartesian coordinate)
		/// </summary>
		public double Re
		{
			get => mCore.Re;
			set => mCore.Re = value;
		}

		/// <summary>
		/// Imaginary part of this complex number (cartesian coordinate)
		/// </summary>
		public double Im
		{
			get => mCore.Im;
			set => mCore.Im = value;
		}
		
		/// <summary>
		/// Modulus of this number (polar coordinate)
		/// </summary>
		public double Mod
		{
			get => mCore.Mod;
			set => mCore.Mod = value;
		}

		/// <summary>
		/// Phase of this number, in radians (polar coordinate)
		/// </summary>
		public double Phase
		{
			get => mCore.Phase;
			set => mCore.Phase = value;
		}	

		#endregion

		#region Operator Overloads

		#region Addition Operators

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(cdouble c1, cdouble c2) => new cdouble(c1.Re + c2.Re, c1.Im + c2.Im);
		
		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(cdouble c1, double c2) => new cdouble(c1.Re + c2, c1.Im);

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(cdouble c1, float c2) => new cdouble(c1.Re + c2, c1.Im);

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(cdouble c1, int c2) => new cdouble(c1.Re + c2, c1.Im);

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(cdouble c1, long c2) => new cdouble(c1.Re + c2, c1.Im);		

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(double c1, cdouble c2) => new cdouble(c1 + c2.Re, c2.Im);

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(float c1, cdouble c2) => new cdouble(c1 + c2.Re, c2.Im);

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(int c1, cdouble c2) => new cdouble(c1 + c2.Re, c2.Im);

		/// <summary>
		/// Standard addition - adds real parts and imaginary parts
		/// </summary>
		public static cdouble operator +(long c1, cdouble c2) => new cdouble(c1 + c2.Re, c2.Im);

		#endregion

		#region Subtraction Operators

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(cdouble c1, cdouble c2) => new cdouble(c1.Re - c2.Re, c1.Im - c2.Im);		

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(cdouble c1, double c2) => new cdouble(c1.Re - c2, c1.Im);

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(cdouble c1, float c2) => new cdouble(c1.Re - c2, c1.Im);

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(cdouble c1, int c2) => new cdouble(c1.Re - c2, c1.Im);

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(cdouble c1, long c2) => new cdouble(c1.Re - c2, c1.Im);		

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(double c1, cdouble c2) => new cdouble(c1 - c2.Re, -c2.Im);

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(float c1, cdouble c2) => new cdouble(c1 - c2.Re, -c2.Im);

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(int c1, cdouble c2) => new cdouble(c1 - c2.Re, -c2.Im);

		/// <summary>
		/// Standard subtraction - subtracts real parts and imaginary parts
		/// </summary>
		public static cdouble operator -(long c1, cdouble c2) => new cdouble(c1 - c2.Re, -c2.Im);

		#endregion

		#region Multiplication Operators

		/// <summary>
		/// Standard multiplication, done in the polar form - more efficient than standard
		/// </summary>
		public static cdouble operator *(cdouble c1, cdouble c2)
		{
			var result = new cdouble(c1);
			result.Mod *= c2.Mod;
			result.Phase += c2.Phase;
			return result;
		}

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(cdouble c1, double c2) => new cdouble(c1.Re*c2, c1.Im*c2);

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(cdouble c1, float c2) => new cdouble(c1.Re * c2, c1.Im * c2);

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(cdouble c1, int c2) => new cdouble(c1.Re * c2, c1.Im * c2);

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(cdouble c1, long c2) => new cdouble(c1.Re * c2, c1.Im * c2);

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(double c1, cdouble c2) => new cdouble(c2.Re * c1, c2.Im * c1);

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(float c1, cdouble c2) => new cdouble(c2.Re * c1, c2.Im * c1);

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(int c1, cdouble c2) => new cdouble(c2.Re * c1, c2.Im * c1);

		/// <summary>
		/// Standard multiplication - (a+bi)*c = ac+bci
		/// </summary>
		public static cdouble operator *(long c1, cdouble c2) => new cdouble(c2.Re * c1, c2.Im * c1);

		#endregion

		#region Division Operators

		/// <summary>
		/// Standard division, done in the polar form - it's more efficient
		/// </summary>
		public static cdouble operator /(cdouble c1, cdouble c2)
		{
			var result = new cdouble(c1);
			result.Mod /= c2.Mod;
			result.Phase -= c2.Phase;
			return result;
		}

		/// <summary>
		/// Standard division - (a+bi)/c = a/c + i*b/c
		/// </summary>
		public static cdouble operator /(cdouble c1, double c2) => new cdouble(c1.Re / c2, c1.Im / c2);

		/// <summary>
		/// Standard division - (a+bi)/c = a/c + i*b/c
		/// </summary>
		public static cdouble operator /(cdouble c1, float c2) => new cdouble(c1.Re / c2, c1.Im / c2);

		/// <summary>
		/// Standard division - (a+bi)/c = a/c + i*b/c
		/// </summary>
		public static cdouble operator /(cdouble c1, int c2) => new cdouble(c1.Re / c2, c1.Im / c2);

		/// <summary>
		/// Standard division - (a+bi)/c = a/c + i*b/c
		/// </summary>
		public static cdouble operator /(cdouble c1, long c2) => new cdouble(c1.Re / c2, c1.Im / c2);

		/// <summary>
		/// Standard division, done in the polar form - it's more efficient
		/// </summary>
		public static cdouble operator /(double c1, cdouble c2) => new cdouble(c1) / c2;

		/// <summary>
		/// Standard division, done in the polar form - it's more efficient
		/// </summary>
		public static cdouble operator /(float c1, cdouble c2) => new cdouble(c1) / c2;

		/// <summary>
		/// Standard division, done in the polar form - it's more efficient
		/// </summary>
		public static cdouble operator /(int c1, cdouble c2) => new cdouble(c1) / c2;

		/// <summary>
		/// Standard division, done in the polar form - it's more efficient
		/// </summary>
		public static cdouble operator /(long c1, cdouble c2) => new cdouble(c1) / c2;

		#endregion

		#region Public Methods

		/// <summary>
		/// Returns a new cdouble which is a conjugate of this cdouble
		/// </summary>
		public cdouble Conjugate() => new cdouble(Re, -Im);

		/// <summary>
		/// Returns "Re+Im*i 
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"{Re}" + (Im >= 0 ? "+" : string.Empty) + $"{Im}*i";

		#endregion

		#endregion
	}
}