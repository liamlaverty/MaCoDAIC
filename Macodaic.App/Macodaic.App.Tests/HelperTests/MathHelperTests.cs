using Macodaic.App.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.Tests.HelperTests
{
    [TestClass]
    public class MathHelperTests
    {

        [TestMethod]
        public void Lerp_WhenLerpByIsZero_ReturnsLerpFrom()
        {
            // Arrange
            var lerpFrom = 1;
            var lerpTo = 2;
            var lerpBy = 0;
            // Act
            var result = MathHelper.Lerp(lerpFrom, lerpTo, lerpBy);
            // Assert
            Assert.AreEqual(lerpFrom, result);
        }
        [TestMethod]
        public void Lerp_WhenLerpByIsOne_ReturnsLerpTo()
        {
            // Arrange
            var lerpFrom = 1;
            var lerpTo = 2;
            var lerpBy = 1;
            // Act
            var result = MathHelper.Lerp(lerpFrom, lerpTo, lerpBy);
            // Assert
            Assert.AreEqual(lerpTo, result);
        }
        [TestMethod]
        public void Lerp_WhenLerpByIsHalf_ReturnsHalfwayBetweenLerpFromAndLerpTo()
        {
            // Arrange
            var lerpFrom = 1;
            var lerpTo = 2;
            var lerpBy = 0.5m;
            // Act
            var result = MathHelper.Lerp(lerpFrom, lerpTo, lerpBy);
            // Assert
            Assert.AreEqual(1.5m, result);
        }

        // method to test if lerpBy 0.3 moves the value 30% of the way from 1 to 2
        [TestMethod]
        public void Lerp_WhenLerpByIsThirtyPercent_ReturnsThirtyPercentOfTheWayBetweenLerpFromAndLerpTo()
        {
            // Arrange
            var lerpFrom = 1;
            var lerpTo = 2;
            var lerpBy = 0.3m;
            // Act
            var result = MathHelper.Lerp(lerpFrom, lerpTo, lerpBy);
            // Assert
            Assert.AreEqual(1.3m, result);
        }



        [TestMethod]
        public void Lerp_WhenLerpByIsBelowOne_ReturnsValueBelowLerpTo()
        {
            // Arrange
            var lerpFrom = 1;
            var lerpTo = 2;
            var lerpBy = 0.3m;
            // Act
            var result = MathHelper.Lerp(lerpFrom, lerpTo, lerpBy);
            // Assert
            Assert.IsTrue(result < lerpTo);
        }

        // method to test if lerp approaches lerpTo
        [TestMethod]
        public void Lerp_WhenLerpByIsBelowOne_ReturnsValueApproachingLerpTo()
        {
            // Arrange
            var lerpFrom = 1;
            var lerpTo = 2;
            var lerpBy = 0.3m;
            // Act
            var result = MathHelper.Lerp(lerpFrom, lerpTo, lerpBy);
            for (int i = 0; i < 1000; i++)
            {
                result = MathHelper.Lerp(result, lerpTo, lerpBy);
                Assert.IsTrue(result < lerpTo);
            }
            // Assert
            Assert.IsTrue(result < lerpTo);
        }

    }
}
