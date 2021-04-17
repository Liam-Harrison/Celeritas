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
                },
                {
                    ""name"": ""Build"",
                    ""type"": ""Button"",
                    ""id"": ""fdcd68aa-06d0-413e-a8e8-edf15859db7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
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
                },
                {
                    ""name"": """",
                    ""id"": ""28225b8e-9b6a-4d82-b798-e4cbcc3d429c"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Build"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Console"",
            ""id"": ""875b6a5b-638d-451c-8941-8ce7a939e654"",
            ""actions"": [
                {
                    ""name"": ""Toggle"",
                    ""type"": ""Button"",
                    ""id"": ""0348849f-a555-4845-b721-8db5feedd879"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""438402b3-5d81-48f4-bf53-8894e98b239a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UpBuffer"",
                    ""type"": ""Button"",
                    ""id"": ""6c22b022-b53c-4ba6-9bac-2196244fea0a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DownBuffer"",
                    ""type"": ""Button"",
                    ""id"": ""e8be2aae-4ac8-4823-91b4-1783d3503448"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Focus"",
                    ""type"": ""Button"",
                    ""id"": ""5a3e8d15-5109-46e6-82a5-05e41f6ee981"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c185f25e-a422-442b-a4ea-58c9c3dc6dd7"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""868afe9f-4fde-4ca7-8bca-b80dfee0b918"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ee0dc9f-77c8-441b-a2da-ccae5309a7ba"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""UpBuffer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01ab4e42-b7f4-4432-ba2e-73dbe6a35a02"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""DownBuffer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf422c4c-e88f-4624-bdd4-e56266690814"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Focus"",
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
            m_Basic_Build = m_Basic.FindAction("Build", throwIfNotFound: true);
            // Console
            m_Console = asset.FindActionMap("Console", throwIfNotFound: true);
            m_Console_Toggle = m_Console.FindAction("Toggle", throwIfNotFound: true);
            m_Console_Submit = m_Console.FindAction("Submit", throwIfNotFound: true);
            m_Console_UpBuffer = m_Console.FindAction("UpBuffer", throwIfNotFound: true);
            m_Console_DownBuffer = m_Console.FindAction("DownBuffer", throwIfNotFound: true);
            m_Console_Focus = m_Console.FindAction("Focus", throwIfNotFound: true);
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
        private readonly InputAction m_Basic_Build;
        public struct BasicActions
        {
            private @Actions m_Wrapper;
            public BasicActions(@Actions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Locomotion => m_Wrapper.m_Basic_Locomotion;
            public InputAction @Fire => m_Wrapper.m_Basic_Fire;
            public InputAction @Build => m_Wrapper.m_Basic_Build;
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
                    @Build.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnBuild;
                    @Build.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnBuild;
                    @Build.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnBuild;
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
                    @Build.started += instance.OnBuild;
                    @Build.performed += instance.OnBuild;
                    @Build.canceled += instance.OnBuild;
                }
            }
        }
        public BasicActions @Basic => new BasicActions(this);

        // Console
        private readonly InputActionMap m_Console;
        private IConsoleActions m_ConsoleActionsCallbackInterface;
        private readonly InputAction m_Console_Toggle;
        private readonly InputAction m_Console_Submit;
        private readonly InputAction m_Console_UpBuffer;
        private readonly InputAction m_Console_DownBuffer;
        private readonly InputAction m_Console_Focus;
        public struct ConsoleActions
        {
            private @Actions m_Wrapper;
            public ConsoleActions(@Actions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Toggle => m_Wrapper.m_Console_Toggle;
            public InputAction @Submit => m_Wrapper.m_Console_Submit;
            public InputAction @UpBuffer => m_Wrapper.m_Console_UpBuffer;
            public InputAction @DownBuffer => m_Wrapper.m_Console_DownBuffer;
            public InputAction @Focus => m_Wrapper.m_Console_Focus;
            public InputActionMap Get() { return m_Wrapper.m_Console; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ConsoleActions set) { return set.Get(); }
            public void SetCallbacks(IConsoleActions instance)
            {
                if (m_Wrapper.m_ConsoleActionsCallbackInterface != null)
                {
                    @Toggle.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnToggle;
                    @Toggle.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnToggle;
                    @Toggle.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnToggle;
                    @Submit.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnSubmit;
                    @Submit.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnSubmit;
                    @Submit.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnSubmit;
                    @UpBuffer.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnUpBuffer;
                    @UpBuffer.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnUpBuffer;
                    @UpBuffer.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnUpBuffer;
                    @DownBuffer.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnDownBuffer;
                    @DownBuffer.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnDownBuffer;
                    @DownBuffer.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnDownBuffer;
                    @Focus.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnFocus;
                    @Focus.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnFocus;
                    @Focus.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnFocus;
                }
                m_Wrapper.m_ConsoleActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Toggle.started += instance.OnToggle;
                    @Toggle.performed += instance.OnToggle;
                    @Toggle.canceled += instance.OnToggle;
                    @Submit.started += instance.OnSubmit;
                    @Submit.performed += instance.OnSubmit;
                    @Submit.canceled += instance.OnSubmit;
                    @UpBuffer.started += instance.OnUpBuffer;
                    @UpBuffer.performed += instance.OnUpBuffer;
                    @UpBuffer.canceled += instance.OnUpBuffer;
                    @DownBuffer.started += instance.OnDownBuffer;
                    @DownBuffer.performed += instance.OnDownBuffer;
                    @DownBuffer.canceled += instance.OnDownBuffer;
                    @Focus.started += instance.OnFocus;
                    @Focus.performed += instance.OnFocus;
                    @Focus.canceled += instance.OnFocus;
                }
            }
        }
        public ConsoleActions @Console => new ConsoleActions(this);
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
            void OnBuild(InputAction.CallbackContext context);
        }
        public interface IConsoleActions
        {
            void OnToggle(InputAction.CallbackContext context);
            void OnSubmit(InputAction.CallbackContext context);
            void OnUpBuffer(InputAction.CallbackContext context);
            void OnDownBuffer(InputAction.CallbackContext context);
            void OnFocus(InputAction.CallbackContext context);
        }
    }
}
