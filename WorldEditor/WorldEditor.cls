<?xml version="1.0" encoding="utf-16"?>
<AgileDotNet Version="6.6.0.12">
  <AssemblyList>
    <Assembly Path="Managed\WorldEditor.dll" Secure="false" Obfuscation="false" FlowObfuscation="false" MethodCallObfuscation="false" ILMerge="none">
      <CodeVirtualizationInclusions>
        <MemberSelector Selector="Boolean MoveNext()" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour+&lt;CheckCoroutine&gt;d__10" Type="Method" />
        <MemberSelector Selector="Void OnRustBusterClientDeathScreen(DeathScreenEvent)" Owner="WorldEditor.WorldEditor" Type="Method" />
        <MemberSelector Selector="Void OnRustBusterWeaponFire(BulletWeaponFireEvent)" Owner="WorldEditor.WorldEditor" Type="Method" />
        <MemberSelector Selector="Void .ctor()" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="IEnumerator CheckCoroutine()" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void OnTriggerEnter(Collider)" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void OnTriggerExit(Collider)" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void SetTarget(Transform)" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void Start()" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void StartShootingEffects()" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void StopShootingEffects()" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void Update()" Owner="WorldEditor.ObjectBehaviours.TurretBehaviour" Type="Method" />
        <MemberSelector Selector="Void TurretBehaviour_AddTurretID(UInt32)" Owner="WorldEditor.RPCBehaviour" Type="Method" />
        <MemberSelector Selector="Void TurretBehaviour_Damage(UInt32)" Owner="WorldEditor.RPCBehaviour" Type="Method" />
        <MemberSelector Selector="Void TurretBehaviour_GetInfo(UInt32)" Owner="WorldEditor.RPCBehaviour" Type="Method" />
        <MemberSelector Selector="Void TurretBehaviour_SetTarget(UInt32, String)" Owner="WorldEditor.RPCBehaviour" Type="Method" />
        <MemberSelector Selector="Void TurretBehaviour_SetTargetInRange(UInt32, Boolean)" Owner="WorldEditor.RPCBehaviour" Type="Method" />
      </CodeVirtualizationInclusions>
    </Assembly>
  </AssemblyList>
  <Settings>
    <General OutputDirectory="Managed\Secured" SignatureFile="" PfxPassword="" FilePathMode="relativepath" GenerateDebugInfo="True" SdkPath="" />
    <Obfuscation ObfuscationMapFile="ObfuscationMap" RenamingScheme="printablechars" CrossAssemblyObfuscation="false" ExcludeXamlTypes="false" ControlFlowObfuscation="basic" PredefindSymbolNamesFilePath="" RenameMethodParameters="False">
      <RenamingExclusions />
      <AssemblyLoadPaths />
    </Obfuscation>
    <Secure SecureUserStrings="false" EncryptManagedResources="false" RedistName="AgileDotNetRT" RedistName64="AgileDotNetRT64" DisableRuntimeEmbedding="False" AntiDebuggerDetection="False" SkipSmallMethods="False" />
    <Tracking ConfigureErrorReporting="false" ProductName="AgileDotNet" ProductVersion="6.6.0.12" CompanyName="SecureTeam Software Ltd." />
    <CodeVirtualization PerformCodeVirtualization="true" RedistName="WE_Runtime" />
    <LicenseFeatures />
  </Settings>
  <Licensing>
    <Licenses />
  </Licensing>
</AgileDotNet>