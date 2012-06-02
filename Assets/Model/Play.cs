﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversi.Model
{
    public static class Play
    {
        public const ulong LeftMask = 0x7F7F7F7F7F7F7F7F;
        public const ulong RightMask = 0xFEFEFEFEFEFEFEFE;

        public static Func<ulong, ulong> Left = x => (x >> 1) & LeftMask;
        public static Func<ulong, ulong> Right = x => (x << 1) & RightMask;
        public static Func<ulong, ulong> Up = x => (x >> 8);
        public static Func<ulong, ulong> Down = x => (x << 8);
        public static Func<ulong, ulong> UpLeft = x => (x >> 9) & LeftMask;
        public static Func<ulong, ulong> UpRight = x => (x >> 7) & RightMask;
        public static Func<ulong, ulong> DownRight = x => (x << 9) & RightMask;
        public static Func<ulong, ulong> DownLeft = x => (x << 7) & LeftMask;
        
		public static ulong ValidPlays(ulong playerPieces, ulong opponentPieces, ulong emptySquares)
        {
	        return   ValidateOneDirection(Up, playerPieces, opponentPieces, emptySquares)
	               | ValidateOneDirection(UpRight, playerPieces, opponentPieces, emptySquares)
	               | ValidateOneDirection(Right, playerPieces, opponentPieces, emptySquares)
	               | ValidateOneDirection(DownRight, playerPieces, opponentPieces, emptySquares)
	               | ValidateOneDirection(Down, playerPieces, opponentPieces, emptySquares)
	               | ValidateOneDirection(DownLeft, playerPieces, opponentPieces, emptySquares)
	               | ValidateOneDirection(Left, playerPieces, opponentPieces, emptySquares)
	               | ValidateOneDirection(UpLeft, playerPieces, opponentPieces, emptySquares);
        }

        public static ulong ValidateOneDirection(Func<ulong, ulong> function, ulong playerPieces, ulong opponentPieces, ulong emptySquares)
        {
            var shift = function(playerPieces);
            var potential = shift & opponentPieces;
            ulong validPlays = 0;

            while (potential > 0)
            {
                potential = function(potential);
                validPlays |= potential & emptySquares;
                potential = potential & opponentPieces;
            }
            return validPlays;
        }

        public static ulong PotentialMobilityOneDirection(Func<ulong, ulong> function, ulong playerPieces, ulong emptySquares)
        {
            var shift = function(playerPieces);
            return shift & emptySquares;
            //return potential & emptySquares;
        }

        public static ulong PotentialMobility(ulong playerPieces, ulong emptySquares)
        {
            return PotentialMobilityOneDirection(Up, playerPieces, emptySquares)
                   | PotentialMobilityOneDirection(UpRight, playerPieces, emptySquares)
                   | PotentialMobilityOneDirection(Right, playerPieces, emptySquares)
                   | PotentialMobilityOneDirection(DownRight, playerPieces, emptySquares)
                   | PotentialMobilityOneDirection(Down, playerPieces, emptySquares)
                   | PotentialMobilityOneDirection(DownLeft, playerPieces, emptySquares)
                   | PotentialMobilityOneDirection(Left, playerPieces, emptySquares)
                   | PotentialMobilityOneDirection(UpLeft, playerPieces, emptySquares);
        }

        public static ulong PlacePiece(ulong placement, ulong playerPieces, ulong opponentPieces)
        {
            return   PlaceOneDirection(Up, placement, playerPieces, opponentPieces)
                   | PlaceOneDirection(UpRight, placement, playerPieces, opponentPieces)
                   | PlaceOneDirection(Right, placement, playerPieces, opponentPieces)
                   | PlaceOneDirection(DownRight, placement, playerPieces, opponentPieces)
                   | PlaceOneDirection(Down, placement, playerPieces, opponentPieces)
                   | PlaceOneDirection(DownLeft, placement, playerPieces, opponentPieces) 
                   | PlaceOneDirection(Left, placement, playerPieces, opponentPieces)
                   | PlaceOneDirection(UpLeft, placement, playerPieces, opponentPieces);
        }

        public static ulong PlaceOneDirection(Func<ulong, ulong> function, ulong placement, ulong playerPieces, ulong opponentPieces)
        {
            var potential = function(placement);
            ulong flippedPieces = 0;

            do
            {
                if ((potential & playerPieces) > 0)
                    return flippedPieces;

                potential &= opponentPieces;
                flippedPieces |= potential;
                potential = function(potential);
            }
            while (potential > 0);

            return 0;            
        }
    }
}