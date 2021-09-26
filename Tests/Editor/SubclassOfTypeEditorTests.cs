using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;

namespace TigerDev.SubclassOfType.Tests
{
    public class SubclassOfTypeEditorTests
    {
        private bool IsConsistent<T>(SubclassOf<T> subclass)
        {
            if (subclass.SelectedType != null)
            {
                return subclass.SelectedTypeAssemblyQualifiedName.Equals(subclass.SelectedType.AssemblyQualifiedName)
                    && subclass.SelectedTypeName.Equals(subclass.SelectedType.ToString());
            }
            else
            {
                return subclass.SelectedTypeName.Equals("None");
            }
        }

        [Test]
        public void SubclassOfStringInitializations()
        {
            SubclassOf<Collider> colliderSubclass = new SubclassOf<Collider>();
            Assert.AreEqual(colliderSubclass.SelectedTypeName, "None");
            Assert.AreEqual(colliderSubclass.ParentTypeAssemblyQualifiedName,
                typeof(Collider).AssemblyQualifiedName);
        }

        [Test]
        public void SubclassOfInLineDeclaration()
        {
            SubclassOf<Collider> colliderSubclass = new SubclassOf<Collider>(typeof(MeshCollider));
            Assert.AreEqual(colliderSubclass.SelectedType, typeof(MeshCollider));
            Assert.AreEqual(colliderSubclass.SelectedTypeAssemblyQualifiedName,
                typeof(MeshCollider).AssemblyQualifiedName);
            Assert.AreEqual(colliderSubclass.SelectedTypeName,
                typeof(MeshCollider).ToString());

        }

        [Test]
        public void SubclassOfTypeSetter()
        {
            SubclassOf<Collider> colliderSubclass = new SubclassOf<Collider>();
            Assert.IsTrue(IsConsistent<Collider>(colliderSubclass));
            colliderSubclass.SelectedType = typeof(MeshCollider);
            Assert.IsTrue(IsConsistent<Collider>(colliderSubclass));
            Assert.AreEqual(colliderSubclass.SelectedType, typeof(MeshCollider));
        }

        [Test]
        public void SubsequentAssignments()
        {
            SubclassOf<Collider> colliderSubclass = new SubclassOf<Collider>();
            colliderSubclass.SelectedType = null;
            Assert.AreEqual(colliderSubclass.SelectedType, null);
            
            colliderSubclass.SelectedType = typeof(SphereCollider);
            Assert.AreEqual(colliderSubclass.SelectedType, typeof(SphereCollider));
            Assert.IsTrue(IsConsistent<Collider>(colliderSubclass));

            colliderSubclass.SelectedType = typeof(MeshCollider);
            Assert.AreEqual(colliderSubclass.SelectedType, typeof(MeshCollider));
            Assert.IsTrue(IsConsistent<Collider>(colliderSubclass));

            colliderSubclass.SelectedType = null;
            Assert.AreEqual(colliderSubclass.SelectedType, null);
            Assert.IsTrue(IsConsistent<Collider>(colliderSubclass));
        }

        [Test]
        public void EqualTest()
        {
            SubclassOf<Collider> colliderSubclass = new SubclassOf<Collider>(typeof(MeshCollider));
            SubclassOf<Collider> anotherColliderSubclass = new SubclassOf<Collider>(typeof(SphereCollider));
            Assert.IsFalse(colliderSubclass.Equals(anotherColliderSubclass));
            anotherColliderSubclass.SelectedType = null;
            Assert.IsFalse(colliderSubclass.Equals(anotherColliderSubclass));
            anotherColliderSubclass.SelectedType = typeof(MeshCollider);
            Assert.IsTrue(colliderSubclass.Equals(anotherColliderSubclass));
        }

        [Test]
        public void HashingTest()
        {
            Dictionary<SubclassOf<Collider>, string> presentationMsgs =
                new Dictionary<SubclassOf<Collider>, string>();
            presentationMsgs.Add(new SubclassOf<Collider>(typeof(MeshCollider)), "hello from mesh collider");
            presentationMsgs.Add(new SubclassOf<Collider>(typeof(SphereCollider)), "hello from sphere collider");

            SubclassOf<Collider> meshCollider = new SubclassOf<Collider>(typeof(MeshCollider));
            SubclassOf<Collider> sphereCollider = new SubclassOf<Collider>(typeof(SphereCollider));
            Assert.AreEqual(presentationMsgs[meshCollider], "hello from mesh collider");
            Assert.AreEqual(presentationMsgs[sphereCollider], "hello from sphere collider");
            Assert.IsFalse(presentationMsgs.ContainsKey(new SubclassOf<Collider>()));
        }
    }
}
