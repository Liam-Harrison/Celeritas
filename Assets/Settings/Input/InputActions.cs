// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Celeritas
{
    public class @InputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Basic"",
            ""id"": ""a5a6cbb2-d233-4086-b5f9-bd4e9905bee2"",
            ""actions"": [
                {
                    ""name"": ""Move"",
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
                },
                {
                    ""name"": ""Ability 1"",
                    ""type"": ""Button"",
                    ""id"": ""a8104ea1-d7a9-4a37-b683-cf4214f4c126"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability 2"",
                    ""type"": ""Button"",
                    ""id"": ""27ce111f-4c66-43cf-ab30-2b35e3f81c5a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability 3"",
                    ""type"": ""Button"",
                    ""id"": ""1e9b049c-2004-4835-a083-73f713505710"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability 4"",
                    ""type"": ""Button"",
                    ""id"": ""770d712b-4f41-40aa-87a4-12441485f7a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Alternate Abilities"",
                    ""type"": ""Button"",
                    ""id"": ""7bec8b9d-b59b-43e2-b28e-76f45762d85d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tractor Beam"",
                    ""type"": ""Button"",
                    ""id"": ""48764d58-2f97-4c48-bd99-3def421e5ee6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle Tutorial"",
                    ""type"": ""Button"",
                    ""id"": ""4e4119ed-efb7-4739-86a5-5a3f10327aea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""61d4ec2c-43eb-4b8b-a32e-a24eff921bcb"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3529d6df-ded3-44a3-8c6e-47a772a6894c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
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
                    ""action"": ""Move"",
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
                    ""action"": ""Move"",
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
                    ""action"": ""Move"",
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
                    ""action"": ""Move"",
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
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Build"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba885465-5fae-4a0d-8e35-508be89fad19"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Ability 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec2373e5-52fe-4a1a-a986-734cad65f35d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Tractor Beam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82503b2c-eaa9-4104-bf13-6f91bbd68122"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Tutorial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07610bc5-6ed4-460b-8048-102e212e5203"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Ability 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e75c58e-d9a4-4837-8724-a099dbf29d4d"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Ability 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe935ce8-fdee-4ef7-b3f1-c48c6a3d212c"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Ability 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b43fb0f-7ba0-440e-bdbe-3bed44cd11fd"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Alternate Abilities"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8463639-b075-4af8-9505-5af5a91bc9a5"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Navigation"",
            ""id"": ""3b84d1a5-c919-4cbd-ac8e-0adc7cebcdf8"",
            ""actions"": [
                {
                    ""name"": ""Navigate UI"",
                    ""type"": ""Button"",
                    ""id"": ""c40002b0-4065-4a64-b3c7-f603edeef5c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause Menu"",
                    ""type"": ""Button"",
                    ""id"": ""b868e02d-6ee8-4f2f-90b8-a309f47af83e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6ed45e5d-7a63-41ab-822d-4aa60e3e90a4"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Navigate UI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bd93bc7-be11-4381-bd31-c879c10d0fca"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""Pause Menu"",
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
            m_Basic_Move = m_Basic.FindAction("Move", throwIfNotFound: true);
            m_Basic_Fire = m_Basic.FindAction("Fire", throwIfNotFound: true);
            m_Basic_Build = m_Basic.FindAction("Build", throwIfNotFound: true);
            m_Basic_Ability1 = m_Basic.FindAction("Ability 1", throwIfNotFound: true);
            m_Basic_Ability2 = m_Basic.FindAction("Ability 2", throwIfNotFound: true);
            m_Basic_Ability3 = m_Basic.FindAction("Ability 3", throwIfNotFound: true);
            m_Basic_Ability4 = m_Basic.FindAction("Ability 4", throwIfNotFound: true);
            m_Basic_AlternateAbilities = m_Basic.FindAction("Alternate Abilities", throwIfNotFound: true);
            m_Basic_TractorBeam = m_Basic.FindAction("Tractor Beam", throwIfNotFound: true);
            m_Basic_ToggleTutorial = m_Basic.FindAction("Toggle Tutorial", throwIfNotFound: true);
            m_Basic_Zoom = m_Basic.FindAction("Zoom", throwIfNotFound: true);
            // Navigation
            m_Navigation = asset.FindActionMap("Navigation", throwIfNotFound: true);
            m_Navigation_NavigateUI = m_Navigation.FindAction("Navigate UI", throwIfNotFound: true);
            m_Navigation_PauseMenu = m_Navigation.FindAction("Pause Menu", throwIfNotFound: true);
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
        private readonly InputAction m_Basic_Move;
        private readonly InputAction m_Basic_Fire;
        private readonly InputAction m_Basic_Build;
        private readonly InputAction m_Basic_Ability1;
        private readonly InputAction m_Basic_Ability2;
        private readonly InputAction m_Basic_Ability3;
        private readonly InputAction m_Basic_Ability4;
        private readonly InputAction m_Basic_AlternateAbilities;
        private readonly InputAction m_Basic_TractorBeam;
        private readonly InputAction m_Basic_ToggleTutorial;
        private readonly InputAction m_Basic_Zoom;
        public struct BasicActions
        {
            private @InputActions m_Wrapper;
            public BasicActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Basic_Move;
            public InputAction @Fire => m_Wrapper.m_Basic_Fire;
            public InputAction @Build => m_Wrapper.m_Basic_Build;
            public InputAction @Ability1 => m_Wrapper.m_Basic_Ability1;
            public InputAction @Ability2 => m_Wrapper.m_Basic_Ability2;
            public InputAction @Ability3 => m_Wrapper.m_Basic_Ability3;
            public InputAction @Ability4 => m_Wrapper.m_Basic_Ability4;
            public InputAction @AlternateAbilities => m_Wrapper.m_Basic_AlternateAbilities;
            public InputAction @TractorBeam => m_Wrapper.m_Basic_TractorBeam;
            public InputAction @ToggleTutorial => m_Wrapper.m_Basic_ToggleTutorial;
            public InputAction @Zoom => m_Wrapper.m_Basic_Zoom;
            public InputActionMap Get() { return m_Wrapper.m_Basic; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BasicActions set) { return set.Get(); }
            public void SetCallbacks(IBasicActions instance)
            {
                if (m_Wrapper.m_BasicActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnMove;
                    @Fire.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnFire;
                    @Fire.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnFire;
                    @Fire.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnFire;
                    @Build.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnBuild;
                    @Build.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnBuild;
                    @Build.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnBuild;
                    @Ability1.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility1;
                    @Ability1.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility1;
                    @Ability1.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility1;
                    @Ability2.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility2;
                    @Ability2.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility2;
                    @Ability2.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility2;
                    @Ability3.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility3;
                    @Ability3.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility3;
                    @Ability3.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility3;
                    @Ability4.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility4;
                    @Ability4.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility4;
                    @Ability4.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnAbility4;
                    @AlternateAbilities.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnAlternateAbilities;
                    @AlternateAbilities.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnAlternateAbilities;
                    @AlternateAbilities.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnAlternateAbilities;
                    @TractorBeam.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnTractorBeam;
                    @TractorBeam.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnTractorBeam;
                    @TractorBeam.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnTractorBeam;
                    @ToggleTutorial.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnToggleTutorial;
                    @ToggleTutorial.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnToggleTutorial;
                    @ToggleTutorial.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnToggleTutorial;
                    @Zoom.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnZoom;
                    @Zoom.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnZoom;
                    @Zoom.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnZoom;
                }
                m_Wrapper.m_BasicActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Fire.started += instance.OnFire;
                    @Fire.performed += instance.OnFire;
                    @Fire.canceled += instance.OnFire;
                    @Build.started += instance.OnBuild;
                    @Build.performed += instance.OnBuild;
                    @Build.canceled += instance.OnBuild;
                    @Ability1.started += instance.OnAbility1;
                    @Ability1.performed += instance.OnAbility1;
                    @Ability1.canceled += instance.OnAbility1;
                    @Ability2.started += instance.OnAbility2;
                    @Ability2.performed += instance.OnAbility2;
                    @Ability2.canceled += instance.OnAbility2;
                    @Ability3.started += instance.OnAbility3;
                    @Ability3.performed += instance.OnAbility3;
                    @Ability3.canceled += instance.OnAbility3;
                    @Ability4.started += instance.OnAbility4;
                    @Ability4.performed += instance.OnAbility4;
                    @Ability4.canceled += instance.OnAbility4;
                    @AlternateAbilities.started += instance.OnAlternateAbilities;
                    @AlternateAbilities.performed += instance.OnAlternateAbilities;
                    @AlternateAbilities.canceled += instance.OnAlternateAbilities;
                    @TractorBeam.started += instance.OnTractorBeam;
                    @TractorBeam.performed += instance.OnTractorBeam;
                    @TractorBeam.canceled += instance.OnTractorBeam;
                    @ToggleTutorial.started += instance.OnToggleTutorial;
                    @ToggleTutorial.performed += instance.OnToggleTutorial;
                    @ToggleTutorial.canceled += instance.OnToggleTutorial;
                    @Zoom.started += instance.OnZoom;
                    @Zoom.performed += instance.OnZoom;
                    @Zoom.canceled += instance.OnZoom;
                }
            }
        }
        public BasicActions @Basic => new BasicActions(this);

        // Navigation
        private readonly InputActionMap m_Navigation;
        private INavigationActions m_NavigationActionsCallbackInterface;
        private readonly InputAction m_Navigation_NavigateUI;
        private readonly InputAction m_Navigation_PauseMenu;
        public struct NavigationActions
        {
            private @InputActions m_Wrapper;
            public NavigationActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @NavigateUI => m_Wrapper.m_Navigation_NavigateUI;
            public InputAction @PauseMenu => m_Wrapper.m_Navigation_PauseMenu;
            public InputActionMap Get() { return m_Wrapper.m_Navigation; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(NavigationActions set) { return set.Get(); }
            public void SetCallbacks(INavigationActions instance)
            {
                if (m_Wrapper.m_NavigationActionsCallbackInterface != null)
                {
                    @NavigateUI.started -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateUI;
                    @NavigateUI.performed -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateUI;
                    @NavigateUI.canceled -= m_Wrapper.m_NavigationActionsCallbackInterface.OnNavigateUI;
                    @PauseMenu.started -= m_Wrapper.m_NavigationActionsCallbackInterface.OnPauseMenu;
                    @PauseMenu.performed -= m_Wrapper.m_NavigationActionsCallbackInterface.OnPauseMenu;
                    @PauseMenu.canceled -= m_Wrapper.m_NavigationActionsCallbackInterface.OnPauseMenu;
                }
                m_Wrapper.m_NavigationActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @NavigateUI.started += instance.OnNavigateUI;
                    @NavigateUI.performed += instance.OnNavigateUI;
                    @NavigateUI.canceled += instance.OnNavigateUI;
                    @PauseMenu.started += instance.OnPauseMenu;
                    @PauseMenu.performed += instance.OnPauseMenu;
                    @PauseMenu.canceled += instance.OnPauseMenu;
                }
            }
        }
        public NavigationActions @Navigation => new NavigationActions(this);
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
            void OnMove(InputAction.CallbackContext context);
            void OnFire(InputAction.CallbackContext context);
            void OnBuild(InputAction.CallbackContext context);
            void OnAbility1(InputAction.CallbackContext context);
            void OnAbility2(InputAction.CallbackContext context);
            void OnAbility3(InputAction.CallbackContext context);
            void OnAbility4(InputAction.CallbackContext context);
            void OnAlternateAbilities(InputAction.CallbackContext context);
            void OnTractorBeam(InputAction.CallbackContext context);
            void OnToggleTutorial(InputAction.CallbackContext context);
            void OnZoom(InputAction.CallbackContext context);
        }
        public interface INavigationActions
        {
            void OnNavigateUI(InputAction.CallbackContext context);
            void OnPauseMenu(InputAction.CallbackContext context);
        }
    }
}
