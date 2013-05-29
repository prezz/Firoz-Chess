using System;


namespace Chess.Model.Logic
{
    /// <summary>
    /// The Flyweight pattern defines a structure for sharing objects. Applications that use lots of
    /// objects must pay careful attention to the cost of each object. Savings can be had by sharing objects
    /// instead of replicating them. But objects can be shared only if they don't define contect-dependent
    /// state. Flyweight objects have no such state. Any additional information they need to perform their
    /// task is passed to them when needed.
    /// FlyweightPiece is the abstract base class for all flyweight pieces in in the PieceFlyweightFactory.
    /// <seealso cref="Chess.Model.Logic.FlyweightPieceFactory"/>
    /// </summary>
	interface IFlyweightPiece
	{
        /// <summary>
        /// Verifies if a Flyweight piece attacks a square.
        /// </summary>
        /// <param name="board">Board to check attacking on.</param>
        /// <param name="from">Square on which attacking piece is placed.</param>
        /// <param name="to">The square to check if attacked.</param>
        /// <returns>True if a Flyweight placed on "from" square attacks the "to" square. False otherwise.</returns>
        bool Attacks(Board board, Square from, Square to);

        /// <summary>
        /// Generate moves a Flyweight piece can make.
        /// This method does not varifies if a move puts its own king in check.
        /// This is done in MoveManager when adding a move.
        /// </summary>
        /// <param name="board">Board to generate moves for.</param>
        /// <param name="location">Location of the piece.</param>
        /// <param name="moves">Container class to which all generated moves are added.</param>
		void GenerateMoves(Board board, Square location, MoveOrganizer moves);
	}
}
