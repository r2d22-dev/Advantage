using gunlibary;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using GorillaLocomotion;
using BepInEx;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Animations.Rigging;

namespace Avantage.Mods
{
    internal class Movement
    {
        public static float Speed = 24f;
        public static void SpeedBoost()
        {
            GorillaLocomotion.Player.Instance.maxJumpSpeed = 1.75f;
        }
        public static void Jump()
        {
            Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.back;
            Player.Instance.GetComponent<Rigidbody>().angularVelocity = UnityEngine.Vector3.back;
            Player.Instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 170, ForceMode.Impulse);
            Player.Instance.GetComponent<Rigidbody>().AddForce(GorillaTagger.Instance.offlineVRRig.rightHandPlayer.transform.up * 120f, ForceMode.Impulse);
        }
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton || UnityInput.Current.GetKey(KeyCode.E))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Speed;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void TriggerFly()
        {
            if (ClientInput.GetInputValue(ControllerInputPoller.TriggerFloat(XRNode.RightHand)))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Speed;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void SpiderMan()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (!leftgrapple)
                {
                    leftgrapple = true;
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.leftHandTransform.forward * 5f;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            true,
                            999999f
                        });
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, true, 999999f);
                    }
                    RaycastHit lefthit;
                    if (Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out lefthit, 512f))
                    {
                        leftgrapplehook = lefthit.point;

                        leftspring = GorillaTagger.Instance.gameObject.AddComponent<SpringJoint>();
                        leftspring.autoConfigureConnectedAnchor = false;
                        leftspring.connectedAnchor = leftgrapplehook;

                        float leftdistanceFromPoint = Vector3.Distance(GorillaTagger.Instance.bodyCollider.attachedRigidbody.position, leftgrapplehook);

                        leftspring.maxDistance = leftdistanceFromPoint * 0.8f;
                        leftspring.minDistance = leftdistanceFromPoint * 0.25f;

                        leftspring.spring = 10f;
                        leftspring.damper = 50f;
                        leftspring.massScale = 12f;
                    }
                }

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.red;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.leftHandTransform.position);
                liner.SetPosition(1, leftgrapplehook);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                line.AddComponent<GunTemplate>().StartCoroutine(GunTemplate.StartCurvyLineRenderer(liner, GorillaTagger.Instance.rightHandTransform.position, GunTemplate.lr, GunTemplate.spherepointer.transform.position));
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out var Ray, 512f);
                GunTemplate.spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GunTemplate.spherepointer.AddComponent<Renderer>();
                GunTemplate.spherepointer.transform.localScale = new Vector3(0.040f, 0.040f, 0.040f);
                GunTemplate.spherepointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                GameObject.Destroy(GunTemplate.spherepointer.GetComponent<BoxCollider>());
                GameObject.Destroy(GunTemplate.spherepointer.GetComponent<Rigidbody>());
                GameObject.Destroy(GunTemplate.spherepointer.GetComponent<Collider>());
                GunTemplate.lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                GunTemplate.spherepointer.AddComponent<GunTemplate>().StartCoroutine(GunTemplate.PulsePointer(GunTemplate.spherepointer));

                GunTemplate.lr = Vector3.Lerp(GunTemplate.lr, (GorillaTagger.Instance.rightHandTransform.position + GunTemplate.spherepointer.transform.position) / 2f, Time.deltaTime * 6f);
                GameObject gameObject = new GameObject("Line");
                LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.010f;
                lineRenderer.endWidth = 0.010f;
                lineRenderer.startColor = GunTemplate.LineColor;
                lineRenderer.endColor = GunTemplate.LineColor;
                lineRenderer.useWorldSpace = true;
                lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));
                gameObject.AddComponent<GunTemplate>().StartCoroutine(GunTemplate.StartCurvyLineRenderer(lineRenderer, GorillaTagger.Instance.rightHandTransform.position, GunTemplate.lr, GunTemplate.spherepointer.transform.position));
                GameObject.Destroy(lineRenderer, Time.deltaTime);

                leftgrapple = false;
                UnityEngine.Object.Destroy(leftspring);
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                if (!righgrapple)
                {
                    righgrapple = true;
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.rightHandTransform.forward * 5f;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            false,
                            999999f
                        });
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, false, 999999f);
                    }
                    RaycastHit righthit;
                    if (Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out righthit, 512f))
                    {
                        rightgrappblehook = righthit.point;

                        rightspring = GorillaTagger.Instance.gameObject.AddComponent<SpringJoint>();
                        rightspring.autoConfigureConnectedAnchor = false;
                        rightspring.connectedAnchor = rightgrappblehook;

                        float rightdistanceFromPoint = Vector3.Distance(GorillaTagger.Instance.bodyCollider.attachedRigidbody.position, rightgrappblehook);

                        rightspring.maxDistance = rightdistanceFromPoint * 0.8f;
                        rightspring.minDistance = rightdistanceFromPoint * 0.25f;

                        rightspring.spring = 10f;
                        rightspring.damper = 50f;
                        rightspring.massScale = 12f;
                    }
                }

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.red;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                liner.SetPosition(1, rightgrappblehook);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out var Ray, 512f);
                GunTemplate.spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GunTemplate.spherepointer.AddComponent<Renderer>();
                GunTemplate.spherepointer.transform.localScale = new Vector3(0.040f, 0.040f, 0.040f);
                GunTemplate.spherepointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                GameObject.Destroy(GunTemplate.spherepointer.GetComponent<BoxCollider>());
                GameObject.Destroy(GunTemplate.spherepointer.GetComponent<Rigidbody>());
                GameObject.Destroy(GunTemplate.spherepointer.GetComponent<Collider>());
                GunTemplate.lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                GunTemplate.spherepointer.AddComponent<GunTemplate>().StartCoroutine(GunTemplate.PulsePointer(GunTemplate.spherepointer));

                GunTemplate.lr = Vector3.Lerp(GunTemplate.lr, (GorillaTagger.Instance.rightHandTransform.position + GunTemplate.spherepointer.transform.position) / 2f, Time.deltaTime * 6f);
                GameObject gameObject = new GameObject("Line");
                LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.010f;
                lineRenderer.endWidth = 0.010f;
                lineRenderer.startColor = GunTemplate.LineColor;
                lineRenderer.endColor = GunTemplate.LineColor;
                lineRenderer.useWorldSpace = true;
                lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));
                gameObject.AddComponent<GunTemplate>().StartCoroutine(GunTemplate.StartCurvyLineRenderer(lineRenderer, GorillaTagger.Instance.rightHandTransform.position, GunTemplate.lr, GunTemplate.spherepointer.transform.position));
                GameObject.Destroy(lineRenderer, Time.deltaTime);
                righgrapple = false;
                UnityEngine.Object.Destroy(rightspring);
            }
        }
        public static Vector3 rightgrappblehook;
        public static Vector3 leftgrapplehook;
        public static SpringJoint rightspring;
        public static SpringJoint leftspring;
        public static bool leftgrapple = false;
        public static bool righgrapple = false;
    }
}
