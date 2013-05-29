using System;


namespace Chess.Model.Engine
{
	enum TranspositionFlag
	{
		NoScore,
		ExactScore,
		AlphaScore,
		BetaScore
	};
}
