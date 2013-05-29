using System;


namespace Chess.Model
{
  /// <summary>
  /// Exception class thrown when an exception happens in the chess assambly.
  /// </summary>
  class ChessModelException : ApplicationException
  {
    /// <summary>
    /// Initializes a new instance of the ChessModelException.
    /// </summary>
    internal ChessModelException()
      : base()
    { }

    /// <summary>
    /// Initializes a new instance of the ChessModelException.
    /// </summary>
    /// <param name="message">A message that descripes the error.</param>
    internal ChessModelException(string message)
      : base(message)
    { }
  }
}
