// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/Actions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Celeritas
{
    public class @Actions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Actions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Actions"",
    ""maps"": [
        {
            ""name"": ""Basic"",
            ""id"": ""a5a6cbb2-d233-4086-b5f9-bd4e9905bee2"",
            ""actions"": [
                {
                    ""name"": ""Locomotion"",
                    ""type"": ""Value"",
                    ""id"": ""b04b367e-c4c0-4a5a-bc67-ce7d3d1aa72c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""b3daf651-fd26-4bbc-8676-ca7cc8085f24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""38b44b86-cc99-4407-aa27-08184374da02"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""004c9eb3-0260-464e-8b3b-d5e9b41c35e3"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3529d6df-ded3-44a3-8c6e-47a772a6894c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""79e411dc-6bab-46da-8bf9-799c62e370f8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f7a2b0e1-340d-4f94-aa15-424722b520c0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""05f51af7-2a74-4878-84e8-f86c38e107dc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3705ac67-0c2e-4df9-b96e-a1b00a17da71"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""06688882-c4c7-4b42-9208-ca1cfd220cdd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controls"",
            ""bindingGroup"": ""Controls"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Basic
            m_Basic = asset.FindActionMap("Basic", throwIfNotFound: true);
            m_Basic_Locomotion = m_Basic.FindAction("Locomotion", throwIfNotFound: true);
            m_Basic_Fire = m_Basic.FindAction("Fire", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Basic
        private readonly InputActionMap m_Basic;
        private IBasicActions m_BasicActionsCallbackInterface;
        private readonly InputAction m_Basic_Locomotion;
        private readonly InputAction m_Basic_Fire;
        public struct BasicActions
        {
            private @Actions m_Wrapper;
            public BasicActions(@Actions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Locomotion => m_Wrapper.m_Basic_Locomotion;
            public InputAction @Fire => m_Wrapper.m_Basic_Fire;
            public InputActionMap Get() { return m_Wrapper.m_Basic; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BasicActions set) { return set.Get(); }
            public void SetCallbacks(IBasicActions instance)
            {
                if (m_Wrapper.m_BasicActionsCallbackInterface != null)
                {
                    @Locomotion.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnLocomotion;
                    @Locomotion.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnLocomotion;
                    @Locomotion.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnLocomotion;
                    @Fire.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnFire;
                    @Fire.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnFire;
                    @Fire.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnFire;
                }
                m_Wrapper.m_BasicActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Locomotion.started += instance.OnLocomotion;
                    @Locomotion.performed += instance.OnLocomotion;
                    @Locomotion.canceled += instance.OnLocomotion;
                    @Fire.started += instance.OnFire;
                    @Fire.performed += instance.OnFire;
                    @Fire.canceled += instance.OnFire;
                }
            }
        }
        public BasicActions @Basic => new BasicActions(this);
        private int m_ControlsSchemeIndex = -1;
        public InputControlScheme ControlsScheme
        {
            get
            {
                if (m_ControlsSchemeIndex == -1) m_ControlsSchemeIndex = asset.FindControlSchemeIndex("Controls");
                return asset.controlSchemes[m_ControlsSchemeIndex];
            }
        }
        public interface IBasicActions
        {
            void OnLocomotion(InputAction.CallbackContext context);
            void OnFire(InputAction.CallbackContext context);
        }
    }
}
