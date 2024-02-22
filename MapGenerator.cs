using LaserGolf.Components;
using LaserGolf.Components.Obstacles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;


namespace LaserGolf.Maps
{
    /// <summary>
    /// Enum which holds the different types that map can be. Used for defining what kind of obstacles and structure a procedurally created map should have.
    /// </summary>
    public enum MapTypes
    {
        TUNNEL,
        MAZE
    }

    /// <summary>
    /// Represents an object/element placed on a <see cref="Map"/>. Can be obstacles, the start point, or the hole.
    /// </summary>


    /// <summary>
    /// This class is used to represent a map that can be created by the game.  Holds information on what kind of obstacles are on the map and where they are.
    /// Handles creating a procedurally generated map or can serve a set of pre built maps.
    /// </summary>
    internal class Map
    {
        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        private List<LaserGolfObstacle> _obstacles;

        /// <summary>
        /// The hole (end) component of this Map
        /// </summary>
        private GolfHole _hole;

        /// <summary>
        /// Vector holding where on the map the ball starts
        /// </summary>
        private Vector2 _ballPos;

        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        public List<LaserGolfObstacle> Obstacles { get { return _obstacles; } }



        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        public GolfHole Hole { get { return _hole; } }

        /// <summary>
        /// Vector holding where on the map the ball starts
        /// </summary>
        public Vector2 BallPos
        {
            get { return _ballPos; }
            set { _ballPos = value; }
        }

        /// <summary>
        /// Create a procedurally generated map
        /// </summary>
        public Map(Game game, double screenRatioMult)
        {
            _obstacles = new List<LaserGolfObstacle>();
            generateTunnelMap(game, screenRatioMult);
        }

        /// <summary>
        /// Create a Map with a premade layout
        /// </summary>
	    /// <param name="game"> The Game object the components of this Map will exist on</param>
        /// <param name="preset">Number corresponding to the layout you would like.
        /// Layouts:
        /// 0 = Standard tunnel map
        /// </param>
        /// <param name="screenRatioMult"> A number which you can multiply the width of the play screen by to get the height. 
        /// Used to prevent stretching when scaling
        /// </param>

        public Map(Game game, int preset, double screenRatioMult)
        {
            _obstacles = new List<LaserGolfObstacle>();

            if (preset == 0)
            {
                createStandardTunnel(game, screenRatioMult);
            }
        }

        /// <summary>
        /// Function to create the standard tunnel preset (Preset 0)
        /// </summary>
        private void createStandardTunnel(Game game, double screenRatioMult)
        {
            int lineSize = 8;

            int screenSize = 100;

            // Set the far left wall for the tunnel

            // Calculate the locations for the wall by percentage of screen and round them
            int wallPosX = (int)System.Math.Round(screenSize * 0.1, 0);
            int wallPosY = (int)System.Math.Round(screenSize * 0.1, 0);
            int wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall leftWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            leftWall.ScaleHeight = (int)Math.Round(screenSize * screenRatioMult);
            leftWall.ScaleX = screenSize;
            leftWall.ScaleY = (int)Math.Round(screenSize * screenRatioMult);

            // Set the lower right wall of the tunnel start

            // Shift the starting position and size for the wall
            wallPosX += ((screenSize / 40) * 3);
            wallPosY += ((screenSize / 40) * 3);// + (lineSize/ screenSize);
            wallSize -= ((screenSize / 40) * 3) + (lineSize / screenSize);

            // Create the wall Object
            Wall rightWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            rightWall.ScaleHeight = (int)Math.Round(screenSize * screenRatioMult);
            rightWall.ScaleX = screenSize;
            rightWall.ScaleY = (int)Math.Round(screenSize * screenRatioMult);


            // Create A wall to enclose the top of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int)(System.Math.Round(screenSize * 0.1, 0));
            wallPosY = (int)System.Math.Round((screenSize * 0.1) + (lineSize / screenSize), 0);
            wallSize = (int)System.Math.Round(screenSize * 0.3, 0);

            // Create the Wall Object
            Wall tunnelTop = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            tunnelTop.ScaleWidth = screenSize;
            tunnelTop.ScaleX = screenSize;
            tunnelTop.ScaleY = (int)Math.Round(screenSize * screenRatioMult);


            // Create A wall to enclose the bottok of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int)(System.Math.Round((screenSize * 0.1) + ((screenSize / 40) * 3), 0));
            wallPosY = (int)(System.Math.Round((screenSize * 0.1) + ((screenSize / 40) * 3), 0));
            wallSize = (int)System.Math.Round((screenSize * 0.3) - ((screenSize / 40) * 3), 0);

            // Create the Wall Object
            Wall tunnelBottom = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            tunnelBottom.ScaleWidth = screenSize;
            tunnelBottom.ScaleX = screenSize;
            tunnelBottom.ScaleY = (int)Math.Round(screenSize * screenRatioMult);


            //  Build the end box

            // Build the boxes left top wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round(screenSize * 0.05, 0);
            wallSize = (int)System.Math.Round(screenSize * 0.05, 0);

            // Create the Wall Object
            Wall boxLeftTop = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxLeftTop.ScaleHeight = (int)Math.Round(screenSize * screenRatioMult);
            boxLeftTop.ScaleX = screenSize;
            boxLeftTop.ScaleY = (int)Math.Round(screenSize * screenRatioMult);

            // Build the boxes left bottom wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenSize * 0.1) + ((screenSize / 40) * 3), 0);
            wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall boxLeftBottom = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxLeftBottom.ScaleHeight = (int)Math.Round(screenSize * screenRatioMult);
            boxLeftBottom.ScaleX = screenSize;
            boxLeftBottom.ScaleY = (int)Math.Round(screenSize * screenRatioMult);

            // Build the boxes bottom wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenSize * 0.6) + ((screenSize / 40) * 3), 0);
            wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall boxBottom = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            boxBottom.ScaleWidth = screenSize;
            boxBottom.ScaleX = screenSize;
            boxBottom.ScaleY = (int)Math.Round(screenSize * screenRatioMult);

            // Build the boxes top wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenSize * 0.05), 0);
            wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall boxTop = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            boxTop.ScaleWidth = screenSize;
            boxTop.ScaleX = screenSize;
            boxTop.ScaleY = (int)Math.Round(screenSize * screenRatioMult);

            // Build the boxes Right wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.9, 0);
            wallPosY = (int)System.Math.Round(screenSize * 0.05, 0);
            wallSize = (int)System.Math.Round((screenSize * 0.55) + ((screenSize / 40) * 3), 0);

            // Create the Wall Object
            Wall boxRight = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxRight.ScaleHeight = (int)Math.Round(screenSize * screenRatioMult);
            boxRight.ScaleX = screenSize;
            boxRight.ScaleY = (int)Math.Round(screenSize * screenRatioMult);

            // Add a rectangle in the corner for bouncing off of 
            wallPosX = (int)System.Math.Round((screenSize * 0.1) + 4, 0);
            wallPosY = (int)System.Math.Round(screenSize * 0.1, 0);
            wallSize = (int)System.Math.Round(screenSize * 0.075, 0);


            // Create the Wall Object
            Wall bounceWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize, 30);
            bounceWall.ScaleHeight = (int)Math.Round(screenSize * screenRatioMult);
            bounceWall.ScaleX = screenSize;
            bounceWall.ScaleY = (int)Math.Round(screenSize * screenRatioMult);


            // Create borders

            BorderField leftBorder = new BorderField(game, new Point(0, 0), lineSize, screenSize);
            leftBorder.ScaleX = screenSize;
            leftBorder.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            leftBorder.ScaleHeight = screenSize;


            BorderField topBorder = new BorderField(game, new Point(lineSize, 0), screenSize, lineSize);
            topBorder.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            topBorder.ScaleWidth = screenSize;

            BorderField bottomBorder = new BorderField(game, new Point(0, (int)Math.Round(screenSize * 0.99)), screenSize, lineSize);
            bottomBorder.ScaleX = screenSize;
            bottomBorder.ScaleY = screenSize;
            bottomBorder.ScaleWidth = screenSize;

            BorderField rightBorder = new BorderField(game, new Point((int)Math.Round(screenSize * 0.99), 0), lineSize, screenSize);
            rightBorder.ScaleX = screenSize;
            rightBorder.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            rightBorder.ScaleHeight = screenSize;

            // Add Slopes
            float slopeX = (screenSize * 0.1f);
            float slopeY = screenSize * 0.3f;

            Slope upSlope = new Slope(game, new Vector2(slopeX, slopeY), ((screenSize / 40) * 3), ((screenSize / 40) * 3), new Vector2(0, 150));
            upSlope.ScaleWidth = screenSize;
            upSlope.ScaleHeight = screenSize;
            upSlope.ScaleX = screenSize;
            upSlope.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            upSlope.ConstantShifts = new Vector4(8, 0, -8, 0);


            slopeX = (screenSize * 0.3f);
            slopeY = (screenSize * 0.1f) + (lineSize / screenSize);

            Slope downSlope = new Slope(game, new Vector2(slopeX, slopeY), ((screenSize / 40) * 3), ((screenSize / 40) * 3), new Vector2(100, 0));
            downSlope.ScaleWidth = screenSize;
            downSlope.ScaleHeight = screenSize;
            downSlope.ScaleX = screenSize;
            downSlope.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            downSlope.ConstantShifts = new Vector4(0, 8, 0, -8);


            // Create hole
            int holePosX = (int)Math.Round(screenSize * 0.65, 0);
            int holePosY = (int)Math.Round(screenSize * 0.5, 0);
            GolfHole hole = new GolfHole(game, new Point(holePosX, holePosY));
            hole.ScaleX = screenSize;
            hole.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            _hole = hole;

            //  Set ball starting position
            float ballPosX = (float)((screenSize * 0.1) + ((screenSize / 40) * 1.25));
            float ballPosY = screenSize * 0.65F;
            _ballPos = new Vector2(ballPosX, ballPosY);


            // Add the items to the map's obstacle list
            _obstacles.Add(leftWall);
            _obstacles.Add(tunnelTop);
            _obstacles.Add(tunnelBottom);
            _obstacles.Add(rightWall);
            _obstacles.Add(boxLeftTop);
            _obstacles.Add(boxLeftBottom);
            _obstacles.Add(boxBottom);
            _obstacles.Add(boxTop);
            _obstacles.Add(boxRight);
            _obstacles.Add(leftBorder);
            _obstacles.Add(rightBorder);
            _obstacles.Add(topBorder);
            _obstacles.Add(bottomBorder);
            _obstacles.Add(upSlope);
            _obstacles.Add(downSlope);
            _obstacles.Add(bounceWall);


        }


        private void generateTunnelMap(Game game, double screenRatioMult){
            // Constants for map creation
            int lineSize = 8;
            int screenSize = 100;
            double screenHeight = screenRatioMult * screenSize;
            int ballWidth = screenSize / 40;

            // Define where the play area starts and ends
            int playXStart = 9;
            int playXEnd = screenSize - 9;

            int tunnelSize = (screenSize / 40) * 3;

            //Make random choices about the map

            //  Randomly choose start location -- it will always be on the left side of the screen
            Random rand = new Random();
            int startX = rand.Next(playXStart + ballWidth, (int) Math.Round(screenSize * 0.2));
            int startY = rand.Next(ballWidth + tunnelSize, (int) Math.Round(screenHeight) - ballWidth - tunnelSize);
            _ballPos = new Vector2(startX, startY);

            // Place the hole on the opposite side and draw box around it -- The size of the box will be randomly choses
            int holeX = rand.Next((int)Math.Round(screenSize * 0.6), (playXEnd - ballWidth) - 11);
            int holeY = rand.Next(ballWidth + lineSize, (int)Math.Round(screenHeight - (ballWidth + lineSize)));


            // Calculate where in the box height wise to place the hole 
            int wallUpperY = rand.Next((screenSize / 40), holeY + (screenSize / 40));

            //Randomly choose the dimensions of the box (from a minimum of three ball widths to a maximum of ~half the screen length wise)
            // Height wise it can cover most of the screen but will leave enough of a gap for the ball to pass on both sides
            int maxWidth = Math.Min(screenSize / 2, (playXEnd - holeX) * 2);
            int minWidth = Math.Max(3 * (screenSize / 40), 11);
            int boxWidth = rand.Next(minWidth, maxWidth);

            int maxHeight = (int) Math.Round((screenHeight - wallUpperY) - ballWidth);
            int minHeight = Math.Max((holeY + ballWidth) - wallUpperY, 11);
            int boxHeight = rand.Next(minHeight, maxHeight);

            // Randomly choose where to place a wall bank in the box as an obstacle
            int bankHeight = 5;

            int boxStartX = holeX - (boxWidth / 2);
            int bankX = rand.Next(boxStartX + bankHeight, boxStartX + (boxWidth - bankHeight));
            int bankY = rand.Next(wallUpperY + bankHeight, wallUpperY + (boxHeight - bankHeight));

            // Get a random rotation to the bank
            float rot = rand.Next(-60, 60);

            // Make sure the bank doesn't overlap the hole

            //Calulcate thew x and y of the otherside of the bank 
            double rotatedX = bankX + (bankHeight * Math.Cos(Ball.toRadians(90 + rot)));
            double rotatedY = bankY + (bankHeight * Math.Sin(Ball.toRadians(90 + rot)));
            
            // If the bank does overlap move it
            // Check if the hole x positon is between the two ends of the bank
            if((holeX <= rotatedX && holeX >= bankX) || (holeX >= rotatedX && holeX <= bankX))
            {
                // Check if its within the y
                if ((holeY <= rotatedY && holeY >= bankY) || (holeY >= rotatedY && holeY <= bankY))
                {
                    // Shift the bank
                    bankX -= bankHeight;
                }

            }

            // Create the bank object
            Wall bank = new Wall(game, new Point(bankX, bankY), lineSize, bankHeight, 30);
            bank.ScaleHeight = screenSize;
            bank.ScaleX = screenSize;
            bank.ScaleY = screenHeight;

            _obstacles.Add(bank);


            // Create hole and the wall's of the box objects
            // Create hole
            GolfHole hole = new GolfHole(game, new Point(holeX, holeY));
            hole.ScaleX = screenSize;
            hole.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            _hole = hole;

            // Choose where on the left wall to leave a gap for the Ball/ tunnel
            int gapLoc = rand.Next(wallUpperY, wallUpperY + boxHeight - tunnelSize);

            // Create box objects
            Wall boxTopWall = new Wall(game, new Point(boxStartX, wallUpperY), boxWidth, lineSize);
            boxTopWall.ScaleWidth = screenSize;
            boxTopWall.ScaleX = screenSize;
            boxTopWall.ScaleY = screenHeight;
            _obstacles.Add(boxTopWall);

            Wall boxUpperLeftWall = new Wall(game, new Point(boxStartX, wallUpperY), lineSize, gapLoc - wallUpperY);
            boxUpperLeftWall.ScaleHeight = screenHeight;
            boxUpperLeftWall.ScaleX = screenSize;
            boxUpperLeftWall.ScaleY = screenHeight;
            _obstacles.Add(boxUpperLeftWall);

            Wall boxLowerLeftWall = new Wall(game, new Point(boxStartX, gapLoc + tunnelSize), lineSize, boxHeight - ((gapLoc - wallUpperY) + tunnelSize));
            boxLowerLeftWall.ScaleHeight = screenHeight;
            boxLowerLeftWall.ScaleX = screenSize;
            boxLowerLeftWall.ScaleY = screenHeight;
            _obstacles.Add(boxLowerLeftWall);

            Wall boxBottom = new Wall(game, new Point(boxStartX, wallUpperY + boxHeight), boxWidth, lineSize);
            boxBottom.ScaleWidth = screenSize;
            boxBottom.ScaleX = screenSize;
            boxBottom.ScaleY = screenHeight;
            _obstacles.Add(boxBottom);

            Wall rightWall = new Wall(game, new Point(boxStartX + boxWidth, wallUpperY), lineSize, boxHeight);
            rightWall.ScaleHeight = screenHeight;
            rightWall.ScaleX = screenSize;
            rightWall.ScaleY = screenHeight;
            rightWall.ConstantShifts = new Vector4(0, 0, 0, 8);
            _obstacles.Add(rightWall);



            // Create borders
            BorderField leftBorder = new BorderField(game, new Point(0, 0), lineSize, screenSize);
            leftBorder.ScaleX = screenSize;
            leftBorder.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            leftBorder.ScaleHeight = screenSize;
            _obstacles.Add(leftBorder);


            BorderField topBorder = new BorderField(game, new Point(lineSize, 0), screenSize, lineSize);
            topBorder.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            topBorder.ScaleWidth = screenSize;
            _obstacles.Add(topBorder);


            BorderField bottomBorder = new BorderField(game, new Point(0, (int)Math.Round(screenSize * 0.99)), screenSize, lineSize);
            bottomBorder.ScaleX = screenSize;
            bottomBorder.ScaleY = screenSize;
            bottomBorder.ScaleWidth = screenSize;
            _obstacles.Add(bottomBorder);


            BorderField rightBorder = new BorderField(game, new Point((int)Math.Round(screenSize * 0.99), 0), lineSize, screenSize);
            rightBorder.ScaleX = screenSize;
            rightBorder.ScaleY = (int)Math.Round(screenSize * screenRatioMult);
            rightBorder.ScaleHeight = screenSize;
            _obstacles.Add(rightBorder);

            //Randomly decide where to place slopes 0 is vertical sections of the tunnel, 1 is horizontal sections, 2 is the box
            int upSlopePlace = rand.Next(0, 3);
            int downSlopePlace = rand.Next(0, 3);

            // If they are the same swap one
            if(upSlopePlace == downSlopePlace)
            {
                upSlopePlace = (upSlopePlace + 1) % 3;
            }

            // If one is in the box add it
            if(upSlopePlace == 2)
            {
                // Choose the direction randomly (0 is vertical axis, 1 is horizontal axis)
                int axis = rand.Next(0, 2);

                if(axis == 1)
                {
                    // Randomly choose where in the box the slope is
                    int yPos = rand.Next(wallUpperY, (wallUpperY + boxHeight) - tunnelSize);

                    Slope slope = new Slope(game, new Vector2(boxStartX + (boxWidth / 2), yPos), tunnelSize, tunnelSize, new Vector2(0, 120));
                    slope.ScaleWidth = screenSize;
                    slope.ScaleHeight = screenSize;
                    slope.ScaleX = screenSize;
                    slope.ScaleY = screenHeight;
                    slope.ConstantShifts = new Vector4(lineSize, 0, -1 * lineSize, 0);
                    _obstacles.Add(slope);
                }
                else
                {
                    // Randomly choose where in the box the slope is
                    int xPos = rand.Next(boxStartX, (boxStartX + boxWidth) - tunnelSize);

                    Slope slope = new Slope(game, new Vector2(xPos, wallUpperY + (boxHeight / 2)), tunnelSize, tunnelSize, new Vector2(-120, 0));
                    slope.ScaleWidth = screenSize;
                    slope.ScaleHeight = screenSize;
                    slope.ScaleX = screenSize;
                    slope.ScaleY = screenHeight;
                    slope.ConstantShifts = new Vector4(0, lineSize, 0, -1 * lineSize);
                    _obstacles.Add(slope);

                }
            }
            else if(downSlopePlace == 2) {
                // Choose the direction randomly (0 is vertical axis, 1 is horizontal axis)
                int axis = rand.Next(0, 2);

                if (axis == 1)
                {
                    // Randomly choose where in the box the slope is
                    int yPos = rand.Next(wallUpperY, (wallUpperY + boxHeight) - tunnelSize);

                    Slope slope = new Slope(game, new Vector2(boxStartX + (boxWidth / 2), yPos), tunnelSize, tunnelSize, new Vector2(0, -120));
                    slope.ScaleWidth = screenSize;
                    slope.ScaleHeight = screenSize;
                    slope.ScaleX = screenSize;
                    slope.ScaleY = screenHeight;
                    slope.ConstantShifts = new Vector4(lineSize, 0, -1 * lineSize, 0);
                    _obstacles.Add(slope);
                }
                else
                {
                    // Randomly choose where in the box the slope is
                    int xPos = rand.Next(boxStartX, (boxStartX + boxWidth) - tunnelSize);

                    Slope slope = new Slope(game, new Vector2(xPos, wallUpperY + (boxHeight / 2)), tunnelSize, tunnelSize, new Vector2(120, 0));
                    slope.ScaleWidth = screenSize;
                    slope.ScaleHeight = screenSize;
                    slope.ScaleX = screenSize;
                    slope.ScaleY = screenHeight;
                    slope.ConstantShifts = new Vector4(0, lineSize, 0, -1 * lineSize);
                    _obstacles.Add(slope);
                }
            }


            // Create the tunnel

            // Track where to add new sections to the tunnel
            Point tunnelLoc = new Point(startX + tunnelSize, startY - (tunnelSize / 2));

            // Get the distance between the start of the tunnel and the gap ( the distance the tunnel needs to cover)
            Point diff = new Point(boxStartX - tunnelLoc.X, gapLoc - tunnelLoc.Y);


            //Var for creating walls
            Wall currSec;


            // Draw the first section of the tunnel --  a partial horizontal advancement

            // Values recorded for placing slopes if necessary
            int recStartX = tunnelLoc.X;
            int recSize;

            //Handle the sizing based on the needed y change
            int topShift;
            int bottomShift;
            if(diff.Y < 0)
            {
                topShift = 0;
                bottomShift = tunnelSize;
            }
            else
            {
                topShift = tunnelSize;
                bottomShift = 0;
            }

            //Randomly choose a percentage to advance
            double percentToAdvance = rand.Next(1, 5) / 100.0f;


            int amtToAdvance = (int)Math.Round(diff.X * percentToAdvance);
            amtToAdvance = Math.Max(amtToAdvance, (int)Math.Round(tunnelSize * 1.5));
            diff.X -= amtToAdvance;

            recSize = amtToAdvance;

            currSec = new Wall(game, new Point(tunnelLoc.X, tunnelLoc.Y), amtToAdvance + topShift, lineSize);
            currSec.ScaleWidth = screenSize;
            currSec.ScaleX = screenSize;
            currSec.ScaleY = screenHeight;
            _obstacles.Add(currSec);

            currSec = new Wall(game, new Point(tunnelLoc.X, tunnelLoc.Y + tunnelSize), amtToAdvance + bottomShift, lineSize);
            currSec.ScaleWidth = screenSize;
            currSec.ScaleX = screenSize;
            currSec.ScaleY = screenHeight;
            _obstacles.Add(currSec);

            tunnelLoc.X += amtToAdvance;
            diff.X -= amtToAdvance;

            // Record the current position for furture use
            int yRec = tunnelLoc.Y;


            // Place slopes if necessary
            if (upSlopePlace == 1)
            {
                // Randomly choose where to place the slope
                int xPos = rand.Next(recStartX + 1, (recStartX + recSize) - tunnelSize);

                Slope slope = new Slope(game, new Vector2(xPos, tunnelLoc.Y), tunnelSize, tunnelSize, new Vector2(-120, 0));
                slope.ScaleWidth = screenSize;
                slope.ScaleHeight = screenSize;
                slope.ScaleX = screenSize;
                slope.ScaleY = screenHeight;
                slope.ConstantShifts = new Vector4(0, 8, 0, -8);
                _obstacles.Add(slope);

            }
            else if (downSlopePlace == 1)
            {
                // Randomly choose where to place the slope
                int xPos = rand.Next(recStartX + 1, (recStartX + recSize) - tunnelSize);

                Slope slope = new Slope(game, new Vector2(xPos, tunnelLoc.Y), tunnelSize, tunnelSize, new Vector2(120, 0));
                slope.ScaleWidth = screenSize;
                slope.ScaleHeight = screenSize;
                slope.ScaleX = screenSize;
                slope.ScaleY = screenHeight;
                slope.ConstantShifts = new Vector4(0, 8, 0, -8);
                _obstacles.Add(slope);

            }


            // Draw the second section of the tunnel - The entire vertical movement

            // user for making decisons direction is needed for
            int diffTracker = diff.Y;

            // Values recorded for placing slopes if necessary
            int recStartY = tunnelLoc.Y;
            recSize = diff.Y;


            // If moving in the upward direction 
            if (diffTracker < 0)
            {
                currSec = new Wall(game, new Point(tunnelLoc.X + tunnelSize, tunnelLoc.Y + diff.Y + tunnelSize), lineSize, Math.Abs(diff.Y));
                currSec.ScaleHeight = screenHeight;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);

                currSec = new Wall(game, new Point(tunnelLoc.X, tunnelLoc.Y + diff.Y), lineSize, Math.Abs(diff.Y));
                currSec.ScaleHeight = screenHeight;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);

                tunnelLoc.Y += diff.Y;
                diff.Y = 0;
            }
            else
            {
                currSec = new Wall(game, new Point(tunnelLoc.X, tunnelLoc.Y + tunnelSize), lineSize, diff.Y);
                currSec.ScaleHeight = screenHeight;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);

                currSec = new Wall(game, new Point(tunnelLoc.X + tunnelSize, tunnelLoc.Y), lineSize, diff.Y);
                currSec.ScaleHeight = screenHeight;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);

                tunnelLoc.Y += diff.Y;
                diff.Y = 0;

            }

            // Place slopes if necessary
            if (upSlopePlace == 0)
            {
                // Randomly choose where to place the slope
                int minVal = Math.Min(recStartY + 1, (recStartY + recSize) - tunnelSize);
                int maxVal = Math.Max(recStartY + 1, (recStartY + recSize) - tunnelSize);
                int yPos = rand.Next(minVal, maxVal);

                Slope slope = new Slope(game, new Vector2(tunnelLoc.X, yPos), tunnelSize, tunnelSize, new Vector2(0, 120));
                slope.ScaleWidth = screenSize;
                slope.ScaleHeight = screenSize;
                slope.ScaleX = screenSize;
                slope.ScaleY = screenHeight;
                slope.ConstantShifts = new Vector4(8, 0, -8, 0);
                _obstacles.Add(slope);
            }
            else if (downSlopePlace == 0)
            {
                // Randomly choose where to place the slope
                int minVal = Math.Min(recStartY + 1, (recStartY + recSize) - tunnelSize);
                int maxVal = Math.Max(recStartY + 1, (recStartY + recSize) - tunnelSize);
                int yPos = rand.Next(minVal, maxVal);

                Slope slope = new Slope(game, new Vector2(tunnelLoc.X, yPos), tunnelSize, tunnelSize, new Vector2(0, -120));
                slope.ScaleWidth = screenSize;
                slope.ScaleHeight = screenSize;
                slope.ScaleX = screenSize;
                slope.ScaleY = screenHeight;
                slope.ConstantShifts = new Vector4(8, 0, -8, 0);
                _obstacles.Add(slope);
            }
            // 90% chance to add a wall at the corner if theres room
            int bounceChoice = rand.Next(10);
            int wallSize = (int)System.Math.Round(screenSize * 0.075, 0);

            if (bounceChoice != 9 && Math.Abs(diffTracker) > wallSize / 2)
            {

                // If its an upwards curve
                if (diffTracker < 0) {

                    // Create the Wall Object
                    Wall bounceWall = new Wall(game, new Point(tunnelLoc.X + tunnelSize, yRec), lineSize, wallSize, 30);
                    bounceWall.ScaleHeight = screenSize;
                    bounceWall.ScaleX = screenSize;
                    bounceWall.ScaleY = screenHeight;
                    bounceWall.ConstantShifts = new Vector4(-8, 0, 0, 0);
                    _obstacles.Add(bounceWall);
                }
                else if(diffTracker > 0)
                {
                    // Create the Wall Object
                    Wall bounceWall = new Wall(game, new Point(tunnelLoc.X + (tunnelSize / 2), yRec), lineSize, wallSize, -30);
                    bounceWall.ScaleHeight = screenSize;
                    bounceWall.ScaleX = screenSize;
                    bounceWall.ScaleY = screenHeight;
                    bounceWall.ConstantShifts = new Vector4(-8, 8, 0, 0);
                    _obstacles.Add(bounceWall);
                }

            }

            // Draw the third section of the tunnel -- Horizontal sections to the gap 


            // If the previous segment was moving up

            if (diffTracker < 0)
            {
                currSec = new Wall(game, new Point(tunnelLoc.X, tunnelLoc.Y), diff.X + tunnelSize + 3, lineSize);
                currSec.ScaleWidth = screenSize;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);

                currSec = new Wall(game, new Point(tunnelLoc.X + tunnelSize, tunnelLoc.Y + tunnelSize), diff.X + 3, lineSize);
                currSec.ScaleWidth = screenSize;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);

            }
            else
            {
                currSec = new Wall(game, new Point(tunnelLoc.X + tunnelSize, tunnelLoc.Y), diff.X + 3, lineSize);
                currSec.ScaleWidth = screenSize;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);

                currSec = new Wall(game, new Point(tunnelLoc.X, tunnelLoc.Y + tunnelSize), diff.X + tunnelSize + 3, lineSize);
                currSec.ScaleWidth = screenSize;
                currSec.ScaleX = screenSize;
                currSec.ScaleY = screenHeight;
                _obstacles.Add(currSec);
            }
           
        }
    }

}
