using System;
using OpenTK;

namespace GrblCNC.Glutils
{
    public class Camera
    {
        Vector3 target, eye;
        float distance, angleHoriz, angleVert;
        Vector3 forward;
        Vector3 right;
        float screenAspect;
        Matrix4 camera;

        public Camera(float aspectRatio)
        {
            screenAspect = aspectRatio;
            SetCameraPosition(new Vector3(18, -10, 10.0f), 40, 250, 22);
        }

        void SetCameraPosition(Vector3 target, float distance, float horizAngle, float vertAngle)
        {
            this.target = target;
            this.distance = distance;
            angleHoriz = horizAngle;
            angleVert = vertAngle;
            UpdateCamera();
        }

        void UpdateCamera()
        {
            if (angleVert > 90)
                angleVert = 90;
            if (angleVert < -90)
                angleVert = -90;
            if (angleHoriz >= 360)
                angleHoriz -= 360;
            if (angleHoriz < 0)
                angleHoriz += 360;
            if (distance < 5)
                distance = 5;
            double angvertrad = MathHelper.DegreesToRadians(angleVert);
            double anghorrad = MathHelper.DegreesToRadians(angleHoriz);
            float distH = (float)(distance * Math.Cos(angvertrad));
            float sinAngHor = (float)Math.Sin(anghorrad);
            float cosAngHor = (float)Math.Cos(anghorrad);
            forward = new Vector3(-cosAngHor, -sinAngHor, 0);
            right = new Vector3(-sinAngHor, cosAngHor, 0);
            float z = (float)(distance * Math.Sin(angvertrad));
            float x = (float)(distH * cosAngHor);
            float y = (float)(distH * sinAngHor);
            eye = new Vector3(x,y,z);
            Vector3 camTop = Vector3.Cross(eye, right);
            camTop.Normalize();
            eye += target;
            camera = Matrix4.LookAt(eye, target, camTop);
            //Matrix4 trans = Matrix4.CreateTranslation(xshift, 0, 0);
            //cam = cam * trans;
            Shader.SetMatrix4All("view", camera);
            UpdateProjection(screenAspect);
        }

        public void UpdateProjection(float aspect)
        {
            screenAspect = aspect;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), aspect, 0.1f, 1000.0f);
            Shader.SetMatrix4All("projection", projection);
        }

        public void Rotate(float anglehor, float anglevert)
        {
            angleHoriz += anglehor;
            angleVert += anglevert;
            UpdateCamera();
        }

        public void Zoom(float dist)
        {
            distance -= dist;
            UpdateCamera();
        }

        public void MoveTarget(float moveright, float moveforward, float moveup)
        {
            if (moveright != 0)
                target += moveright * right;
            if (moveforward != 0)
                target += moveforward * forward;
            if (moveup != 0)
                target.Z += moveup;
            UpdateCamera();
        }


    }
}