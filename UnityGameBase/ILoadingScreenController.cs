using System;

/// <summary>
/// Implement this interface for custom loading screen behaviour. Your implementation needs to be assigned to 
/// SceneTransition::mLoadingScreenController in order to work. 
/// </summary>
public interface ILoadingScreenController
{
	/// <summary>
	/// Gets a value indicating whether this <see cref="ILoadingScreenController"/> is initialized.
	/// </summary>
	/// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
	bool isInitialized{ get; }

	/// <summary>
	/// Initialize this instance. Load and setup prefabs or a scene. 
	/// </summary>
	void Initialize( System.Action pDoneCbk );

	/// <summary>
	/// Called by SceneTransition when you should begin animating the scene transition. 
	/// When your animation is done, you need to call pDoneCbk to actually start the loading process. 
	/// </summary>
	/// <param name="pDoneCbk">Callback for when your animation is finished. </param>
	void OnAnimateInBegin( System.Action pDoneCbk );

	/// <summary>
	/// Called by SceneTransition when you should end animating the scene transition. 
	/// When your animation is done, you need to call pDoneCbk. 
	/// </summary>
	/// <param name="pDoneCbk">Callback for when your animation is finished. .</param>
	void OnAnimateOutBegin( System.Action pDoneCbk );

	/// <summary>
	/// Calley by SceneTransition to decide if the device and license is sufficient to load asynconously. 
	/// </summary>
	/// <returns><c>true</c> if this instance can load async; otherwise, <c>false</c>.</returns>
	bool CanLoadAsync();
}