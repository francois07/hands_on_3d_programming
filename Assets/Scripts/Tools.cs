using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

namespace MyMathTools
{
    [System.Serializable]
    public struct Polar
    {
        public float Rho;
        public float Theta;

        public Polar(Polar pol)
        {
            this.Rho = pol.Rho;
            this.Theta = pol.Theta;
        }

        public Polar(float rho, float theta)
        {
            this.Rho = rho;
            this.Theta = theta;
        }
    }

    [System.Serializable]
    public struct Cylindrical
    {
        public float Rho;
        public float Theta;
        public float y;

        public Cylindrical(Cylindrical cyl)
        {
            this.Rho = cyl.Rho;
            this.Theta = cyl.Theta;
            this.y = cyl.y;
        }

        public Cylindrical(float rho, float theta, float y)
        {
            this.Rho = rho;
            this.Theta = theta;
            this.y = y;
        }
    }

    [System.Serializable]
    public struct Spherical
    {
        public float Rho;
        public float Theta;
        public float Phi;

        public Spherical(Spherical sph)
        {
            this.Rho = sph.Rho;
            this.Theta = sph.Theta;
            this.Phi = sph.Phi;
        }

        public Spherical(float rho, float theta, float phi)
        {
            this.Rho = rho;
            this.Theta = theta;
            this.Phi = phi;
        }
    }

    public static class CoordConvert
    {
        public static Vector2 PolarToCartesian(Polar polar)
        {
            return polar.Rho * new Vector2(Mathf.Cos(polar.Theta), Mathf.Sin(polar.Theta));
        }

        public static Polar CartesianToPolar(Vector2 cart, bool keepThetaPositive = true)
        {
            Polar polar = new Polar(cart.magnitude, 0);
            if (Mathf.Approximately(polar.Rho, 0)) polar.Theta = 0;
            else
            {
                polar.Theta = Mathf.Asin(cart.y / polar.Rho);
                if (cart.x < 0) polar.Theta = Mathf.PI - polar.Theta;
                if (keepThetaPositive && polar.Theta < 0) polar.Theta += 2 * Mathf.PI;
            }
            return polar;
        }

        public static Vector3 SphericalToCartesian(Spherical sph)
        {
            float radius = sph.Rho;
            float inclination = sph.Theta;
            float azimuth = sph.Phi;

            return new Vector3(radius * Mathf.Sin(inclination) * Mathf.Cos(azimuth), radius * Mathf.Sin(inclination) * Mathf.Sin(azimuth), radius * Mathf.Cos(inclination));
        }
    }
}