namespace CSharpEnhanced.CoreInterfaces
{
	/// <summary>
	/// Interface for classes that implement deep copying both ways
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IShallowCopy<T> : IShallowCopyFrom<T>, IShallowCopyTo<T> { }
}