//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_ IslandBoy/Configuration/Input/PlayerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""aef1ee94-f0d5-411f-b0f5-ae2723fb3a96"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""392aa321-187c-4928-a3cd-93ca047ed9e3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PrimaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""f1b0d8ea-5b82-43a7-9956-134932fd41b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.01)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""80c9d6b6-f370-4309-b7d4-4f50a4614a31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.01)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleInventory"",
                    ""type"": ""Button"",
                    ""id"": ""a937fac4-d572-424a-806c-dc59da1416d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""561fd1da-3807-4397-b797-2b463ae328d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpeedDebug"",
                    ""type"": ""Button"",
                    ""id"": ""39ab6aa7-c061-4c7e-a8d2-281bee644bdf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fishing"",
                    ""type"": ""Value"",
                    ""id"": ""6784d131-fb5e-464d-9817-0344163ff9b9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CatchFish"",
                    ""type"": ""Button"",
                    ""id"": ""2dfd4991-4f7d-4fb2-8356-ba127c2b2a84"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""08291065-ac81-46b5-8e9d-8e5aef9a974c"",
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
                    ""id"": ""54d76728-9e30-4ad2-aca8-6281174c165c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6867464d-beea-4c3a-a39d-8717f31b55ad"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ca31eb80-b6ed-4d49-8020-863a1991dd1c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e80cc31d-33a7-48d2-93b6-4923ce4d44d9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6f365307-79ab-41d0-b912-b6eed3e631ce"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73d17aa6-c1ec-4ae6-a6a7-35255fd0f575"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b986f165-7e66-4d31-aa92-145259683aa2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70393449-fa91-49ed-8504-8cb911af719a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d26c0ed-0f56-4f7d-b608-147451a52221"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpeedDebug"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Movement"",
                    ""id"": ""572096e3-ebab-4096-acc0-6dd6e9b79145"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fishing"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""71a01a21-f320-4ee6-b6cd-8a3a4750da66"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fishing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d738f383-24fd-4c6f-bada-dc26ec4fb59f"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fishing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""76fb5f61-3e79-4ac7-80e0-7c8b65e87c77"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CatchFish"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Hotbar"",
            ""id"": ""b8c7f540-cd3d-4206-b61a-ba90148ee45a"",
            ""actions"": [
                {
                    ""name"": ""1"",
                    ""type"": ""Button"",
                    ""id"": ""5ee7f2b9-e175-4d8a-b4a5-40e2a30972eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""2"",
                    ""type"": ""Button"",
                    ""id"": ""9fc45987-4515-458b-8acf-58dbc5fe5bda"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""3"",
                    ""type"": ""Button"",
                    ""id"": ""46e49d2e-100f-4746-9791-1026642b361c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""4"",
                    ""type"": ""Button"",
                    ""id"": ""20a90df0-f8c8-423f-a438-f855789ca6c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""5"",
                    ""type"": ""Button"",
                    ""id"": ""12a5ce3a-483e-4a9a-b1d7-fdff3c1799f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""6"",
                    ""type"": ""Button"",
                    ""id"": ""c9afcabb-aff3-4225-8b66-e70827ae64bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""7"",
                    ""type"": ""Button"",
                    ""id"": ""a97d392b-5c3b-42ea-8eb2-8a213a9e72e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""8"",
                    ""type"": ""Button"",
                    ""id"": ""07a92ff8-18dd-4b98-a209-c55d007f66a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""9"",
                    ""type"": ""Button"",
                    ""id"": ""7480fff3-3740-4d3b-aa37-987f6b4794e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d2b613e7-8b79-48e4-935a-e0cb1332973e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5b328db6-b563-4f83-b2ce-ded41820b04c"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81ccf6ab-026e-4838-9bc4-20eca7b64a7d"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e12ebd3-58ed-41c6-83c6-1d41f358a3c6"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7dcfec96-0919-4c0b-a117-df78594384ab"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40dae07f-b725-486f-9bec-49b809eddeef"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdb52e73-100f-43ab-996d-43a6727e2850"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9de42690-7c91-47e2-baf0-73d06e3ed1cc"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6cc2d94f-066c-4792-a990-9d014d8b6f37"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""8"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e226e836-df8b-4ace-a676-2e831a50b4ef"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""9"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1707a77-54f5-418e-bad1-47d6a3a89bc7"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_PrimaryAction = m_Player.FindAction("PrimaryAction", throwIfNotFound: true);
        m_Player_SecondaryAction = m_Player.FindAction("SecondaryAction", throwIfNotFound: true);
        m_Player_ToggleInventory = m_Player.FindAction("ToggleInventory", throwIfNotFound: true);
        m_Player_PauseMenu = m_Player.FindAction("PauseMenu", throwIfNotFound: true);
        m_Player_SpeedDebug = m_Player.FindAction("SpeedDebug", throwIfNotFound: true);
        m_Player_Fishing = m_Player.FindAction("Fishing", throwIfNotFound: true);
        m_Player_CatchFish = m_Player.FindAction("CatchFish", throwIfNotFound: true);
        // Hotbar
        m_Hotbar = asset.FindActionMap("Hotbar", throwIfNotFound: true);
        m_Hotbar__1 = m_Hotbar.FindAction("1", throwIfNotFound: true);
        m_Hotbar__2 = m_Hotbar.FindAction("2", throwIfNotFound: true);
        m_Hotbar__3 = m_Hotbar.FindAction("3", throwIfNotFound: true);
        m_Hotbar__4 = m_Hotbar.FindAction("4", throwIfNotFound: true);
        m_Hotbar__5 = m_Hotbar.FindAction("5", throwIfNotFound: true);
        m_Hotbar__6 = m_Hotbar.FindAction("6", throwIfNotFound: true);
        m_Hotbar__7 = m_Hotbar.FindAction("7", throwIfNotFound: true);
        m_Hotbar__8 = m_Hotbar.FindAction("8", throwIfNotFound: true);
        m_Hotbar__9 = m_Hotbar.FindAction("9", throwIfNotFound: true);
        m_Hotbar_Scroll = m_Hotbar.FindAction("Scroll", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_PrimaryAction;
    private readonly InputAction m_Player_SecondaryAction;
    private readonly InputAction m_Player_ToggleInventory;
    private readonly InputAction m_Player_PauseMenu;
    private readonly InputAction m_Player_SpeedDebug;
    private readonly InputAction m_Player_Fishing;
    private readonly InputAction m_Player_CatchFish;
    public struct PlayerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @PrimaryAction => m_Wrapper.m_Player_PrimaryAction;
        public InputAction @SecondaryAction => m_Wrapper.m_Player_SecondaryAction;
        public InputAction @ToggleInventory => m_Wrapper.m_Player_ToggleInventory;
        public InputAction @PauseMenu => m_Wrapper.m_Player_PauseMenu;
        public InputAction @SpeedDebug => m_Wrapper.m_Player_SpeedDebug;
        public InputAction @Fishing => m_Wrapper.m_Player_Fishing;
        public InputAction @CatchFish => m_Wrapper.m_Player_CatchFish;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @PrimaryAction.started += instance.OnPrimaryAction;
            @PrimaryAction.performed += instance.OnPrimaryAction;
            @PrimaryAction.canceled += instance.OnPrimaryAction;
            @SecondaryAction.started += instance.OnSecondaryAction;
            @SecondaryAction.performed += instance.OnSecondaryAction;
            @SecondaryAction.canceled += instance.OnSecondaryAction;
            @ToggleInventory.started += instance.OnToggleInventory;
            @ToggleInventory.performed += instance.OnToggleInventory;
            @ToggleInventory.canceled += instance.OnToggleInventory;
            @PauseMenu.started += instance.OnPauseMenu;
            @PauseMenu.performed += instance.OnPauseMenu;
            @PauseMenu.canceled += instance.OnPauseMenu;
            @SpeedDebug.started += instance.OnSpeedDebug;
            @SpeedDebug.performed += instance.OnSpeedDebug;
            @SpeedDebug.canceled += instance.OnSpeedDebug;
            @Fishing.started += instance.OnFishing;
            @Fishing.performed += instance.OnFishing;
            @Fishing.canceled += instance.OnFishing;
            @CatchFish.started += instance.OnCatchFish;
            @CatchFish.performed += instance.OnCatchFish;
            @CatchFish.canceled += instance.OnCatchFish;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @PrimaryAction.started -= instance.OnPrimaryAction;
            @PrimaryAction.performed -= instance.OnPrimaryAction;
            @PrimaryAction.canceled -= instance.OnPrimaryAction;
            @SecondaryAction.started -= instance.OnSecondaryAction;
            @SecondaryAction.performed -= instance.OnSecondaryAction;
            @SecondaryAction.canceled -= instance.OnSecondaryAction;
            @ToggleInventory.started -= instance.OnToggleInventory;
            @ToggleInventory.performed -= instance.OnToggleInventory;
            @ToggleInventory.canceled -= instance.OnToggleInventory;
            @PauseMenu.started -= instance.OnPauseMenu;
            @PauseMenu.performed -= instance.OnPauseMenu;
            @PauseMenu.canceled -= instance.OnPauseMenu;
            @SpeedDebug.started -= instance.OnSpeedDebug;
            @SpeedDebug.performed -= instance.OnSpeedDebug;
            @SpeedDebug.canceled -= instance.OnSpeedDebug;
            @Fishing.started -= instance.OnFishing;
            @Fishing.performed -= instance.OnFishing;
            @Fishing.canceled -= instance.OnFishing;
            @CatchFish.started -= instance.OnCatchFish;
            @CatchFish.performed -= instance.OnCatchFish;
            @CatchFish.canceled -= instance.OnCatchFish;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Hotbar
    private readonly InputActionMap m_Hotbar;
    private List<IHotbarActions> m_HotbarActionsCallbackInterfaces = new List<IHotbarActions>();
    private readonly InputAction m_Hotbar__1;
    private readonly InputAction m_Hotbar__2;
    private readonly InputAction m_Hotbar__3;
    private readonly InputAction m_Hotbar__4;
    private readonly InputAction m_Hotbar__5;
    private readonly InputAction m_Hotbar__6;
    private readonly InputAction m_Hotbar__7;
    private readonly InputAction m_Hotbar__8;
    private readonly InputAction m_Hotbar__9;
    private readonly InputAction m_Hotbar_Scroll;
    public struct HotbarActions
    {
        private @PlayerInput m_Wrapper;
        public HotbarActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @_1 => m_Wrapper.m_Hotbar__1;
        public InputAction @_2 => m_Wrapper.m_Hotbar__2;
        public InputAction @_3 => m_Wrapper.m_Hotbar__3;
        public InputAction @_4 => m_Wrapper.m_Hotbar__4;
        public InputAction @_5 => m_Wrapper.m_Hotbar__5;
        public InputAction @_6 => m_Wrapper.m_Hotbar__6;
        public InputAction @_7 => m_Wrapper.m_Hotbar__7;
        public InputAction @_8 => m_Wrapper.m_Hotbar__8;
        public InputAction @_9 => m_Wrapper.m_Hotbar__9;
        public InputAction @Scroll => m_Wrapper.m_Hotbar_Scroll;
        public InputActionMap Get() { return m_Wrapper.m_Hotbar; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HotbarActions set) { return set.Get(); }
        public void AddCallbacks(IHotbarActions instance)
        {
            if (instance == null || m_Wrapper.m_HotbarActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_HotbarActionsCallbackInterfaces.Add(instance);
            @_1.started += instance.On_1;
            @_1.performed += instance.On_1;
            @_1.canceled += instance.On_1;
            @_2.started += instance.On_2;
            @_2.performed += instance.On_2;
            @_2.canceled += instance.On_2;
            @_3.started += instance.On_3;
            @_3.performed += instance.On_3;
            @_3.canceled += instance.On_3;
            @_4.started += instance.On_4;
            @_4.performed += instance.On_4;
            @_4.canceled += instance.On_4;
            @_5.started += instance.On_5;
            @_5.performed += instance.On_5;
            @_5.canceled += instance.On_5;
            @_6.started += instance.On_6;
            @_6.performed += instance.On_6;
            @_6.canceled += instance.On_6;
            @_7.started += instance.On_7;
            @_7.performed += instance.On_7;
            @_7.canceled += instance.On_7;
            @_8.started += instance.On_8;
            @_8.performed += instance.On_8;
            @_8.canceled += instance.On_8;
            @_9.started += instance.On_9;
            @_9.performed += instance.On_9;
            @_9.canceled += instance.On_9;
            @Scroll.started += instance.OnScroll;
            @Scroll.performed += instance.OnScroll;
            @Scroll.canceled += instance.OnScroll;
        }

        private void UnregisterCallbacks(IHotbarActions instance)
        {
            @_1.started -= instance.On_1;
            @_1.performed -= instance.On_1;
            @_1.canceled -= instance.On_1;
            @_2.started -= instance.On_2;
            @_2.performed -= instance.On_2;
            @_2.canceled -= instance.On_2;
            @_3.started -= instance.On_3;
            @_3.performed -= instance.On_3;
            @_3.canceled -= instance.On_3;
            @_4.started -= instance.On_4;
            @_4.performed -= instance.On_4;
            @_4.canceled -= instance.On_4;
            @_5.started -= instance.On_5;
            @_5.performed -= instance.On_5;
            @_5.canceled -= instance.On_5;
            @_6.started -= instance.On_6;
            @_6.performed -= instance.On_6;
            @_6.canceled -= instance.On_6;
            @_7.started -= instance.On_7;
            @_7.performed -= instance.On_7;
            @_7.canceled -= instance.On_7;
            @_8.started -= instance.On_8;
            @_8.performed -= instance.On_8;
            @_8.canceled -= instance.On_8;
            @_9.started -= instance.On_9;
            @_9.performed -= instance.On_9;
            @_9.canceled -= instance.On_9;
            @Scroll.started -= instance.OnScroll;
            @Scroll.performed -= instance.OnScroll;
            @Scroll.canceled -= instance.OnScroll;
        }

        public void RemoveCallbacks(IHotbarActions instance)
        {
            if (m_Wrapper.m_HotbarActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IHotbarActions instance)
        {
            foreach (var item in m_Wrapper.m_HotbarActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_HotbarActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public HotbarActions @Hotbar => new HotbarActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnPrimaryAction(InputAction.CallbackContext context);
        void OnSecondaryAction(InputAction.CallbackContext context);
        void OnToggleInventory(InputAction.CallbackContext context);
        void OnPauseMenu(InputAction.CallbackContext context);
        void OnSpeedDebug(InputAction.CallbackContext context);
        void OnFishing(InputAction.CallbackContext context);
        void OnCatchFish(InputAction.CallbackContext context);
    }
    public interface IHotbarActions
    {
        void On_1(InputAction.CallbackContext context);
        void On_2(InputAction.CallbackContext context);
        void On_3(InputAction.CallbackContext context);
        void On_4(InputAction.CallbackContext context);
        void On_5(InputAction.CallbackContext context);
        void On_6(InputAction.CallbackContext context);
        void On_7(InputAction.CallbackContext context);
        void On_8(InputAction.CallbackContext context);
        void On_9(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
    }
}
