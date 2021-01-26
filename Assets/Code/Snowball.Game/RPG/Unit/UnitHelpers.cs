using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Snowball.Game
{
	public static class UnitHelpers
	{
		public static void SpawnUnitFacade(
			UnitFacade prefab, Unit unit,
			Vector3Int position,
			Unit.Drivers driver, Unit.Alliances alliance, Unit.Directions direction
		)
		{
			unit.Facade = Object.Instantiate(prefab);
			unit.GridPosition = position;
			unit.Direction = direction;
			unit.Driver = driver;
			unit.Alliance = alliance;

			InitFacade(unit.Facade, unit);
			HideInfos(unit);
		}

		public static Unit.Directions VectorToDirection(Vector3 vector)
		{
			return vector.x > 0 ? Unit.Directions.Right : Unit.Directions.Left;
		}

		public static void ApplyColors(Material material, Color clothColor, Color hairColor, Color skinColor)
		{
			material.SetColor("ReplacementColor1", clothColor);
			material.SetColor("ReplacementColor2", hairColor);
			material.SetColor("ReplacementColor3", skinColor);
		}

		public static void ApplyClothColor(UnitFacade facade, Color clothColor)
		{
			facade.BodyRenderer.material.SetColor("ReplacementColor1", clothColor);
			facade.InfosBodyImage.material.SetColor("ReplacementColor1", clothColor);
		}

		public static Unit Create(UnitAuthoring authoring)
		{
			return new Unit
			{
				Id = authoring.Id,
				Name = authoring.Name,
				Sprite = authoring.Sprite,
				ColorCloth = authoring.ColorCloth,
				ColorHair = authoring.ColorHair,
				ColorSkin = authoring.ColorSkin,
				Type = authoring.Type,
				MoveRange = authoring.MoveRange,
				HealthCurrent = authoring.Health,
				HealthMax = authoring.Health,
				Speed = authoring.Speed,
				HitAccuracy = authoring.HitAccuracy,
				HitRange = authoring.HitRange,
				HitDamage = authoring.HitDamage,
				BuildRange = authoring.BuildRange,
				ActionPatterns = authoring.ActionPatterns,
				Abilities = authoring.Abilities,
			};
		}

		public static void InitFacade(UnitFacade facade, Unit unit)
		{
			SetName(facade, unit.Name);
			ApplyColors(facade.BodyRenderer.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);
			ApplyColors(facade.LeftHandRenderer.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);
			ApplyColors(facade.RightHandRenderer.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);

			facade.transform.position = new Vector3(unit.GridPosition.x, unit.GridPosition.y, 0f);
			facade.BodyRenderer.sprite = unit.Sprite;
			facade.BodyRenderer.transform.Rotate(new Vector3(0f, unit.Direction > 0 ? 0f : 180f, 0f));

			unit.Facade.InfosBodyImage.sprite = unit.Sprite;
			unit.Facade.InfosBodyImage.material = Object.Instantiate(unit.Facade.InfosBodyImage.material);
			ApplyColors(unit.Facade.InfosBodyImage.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);
			ApplyColors(unit.Facade.InfosBodyImage.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);
			ApplyColors(unit.Facade.InfosBodyImage.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);
		}

		public static async UniTask MoveOnPath(UnitFacade facade, List<Vector3Int> path)
		{
			for (var index = 1; index < path.Count; index++)
			{
				await MoveTo(facade, path[index], index % 2 == 0);
			}
		}

		public static async UniTask AnimateChangeDirection(UnitFacade facade, Unit.Directions direction)
		{
			await facade.BodyRenderer.transform.DORotate(new Vector3(0f, direction > 0 ? 0f : 180f, 0f), 0.15f);
		}

		public static async UniTask AnimateAttack(UnitFacade facade, Vector3 aimDirection)
		{
			var origin = facade.RightHandRenderer.transform.position;

			await DOTween.Sequence()
				.Append(facade.LeftHandRenderer.transform.DOMove(origin + aimDirection * 0.2f, 0.1f))
				.Join(facade.RightHandRenderer.transform.DOMove(origin - aimDirection * 0.2f, 0.1f))
				.Append(facade.RightHandRenderer.transform.DOMove(origin + aimDirection * 0.5f, 0.1f))
			;

			_ = facade.LeftHandRenderer.transform.DOMove(origin, 0.1f);
			_ = facade.RightHandRenderer.transform.DOMove(origin, 0.1f);
		}

		public static async UniTask AnimateBuild(UnitFacade facade, Unit.Directions direction)
		{
			var originalPosition = facade.BodyRenderer.transform.position;

			await DOTween.Sequence()
				.Append(facade.LeftHandRenderer.transform.DOLocalMove(facade.LeftHandRenderer.transform.localPosition + new Vector3(0.1f, -0.2f, 0f), 0.1f))
				.Join(facade.RightHandRenderer.transform.DOLocalMove(facade.RightHandRenderer.transform.localPosition + new Vector3(0.1f, -0.2f, 0f), 0.1f))
				.Join(facade.BodyRenderer.transform.DOMoveX(originalPosition.x + (direction == Unit.Directions.Right ? 0.2f : -0.2f), 0.2f))
				.Append(facade.BodyRenderer.transform.DOMoveX(originalPosition.x, 0.1f))
				.Join(facade.LeftHandRenderer.transform.DOLocalMove(facade.LeftHandRenderer.transform.localPosition, 0.1f))
				.Join(facade.RightHandRenderer.transform.DOLocalMove(facade.RightHandRenderer.transform.localPosition, 0.1f));
		}

		public static async UniTask AnimateSpawn(UnitFacade facade)
		{
			facade.BodyRenderer.transform.localScale = Vector3.zero;

			await facade.BodyRenderer.transform.DOScale(1, 0.3f);
		}

		public static async UniTask AnimateDeath(UnitFacade facade)
		{
			await DOTween.Sequence()
				.Append(facade.LeftHandRenderer.transform.DOLocalMove(facade.LeftHandRenderer.transform.localPosition + new Vector3(0.2f, 0.4f, 0f), 0.1f))
				.Join(facade.RightHandRenderer.transform.DOLocalMove(facade.RightHandRenderer.transform.localPosition + new Vector3(-0.2f, 0.4f, 0f), 0.1f))
				.Append(facade.BodyRenderer.transform.DORotate(new Vector3(0f, 0f, 45f), 0.3f))
				.Append(facade.BodyRenderer.transform.DOScale(0, 0.3f))
			;
		}

		public static async UniTask AnimateMelt(UnitFacade facade)
		{
			await facade.BodyPivot.DOScaleY(0, 0.3f);
		}

		public static void PlaySound(UnitFacade facade, AudioClip targetHitClip)
		{
			facade.AudioSource.PlayOneShot(targetHitClip);
		}

		public static async UniTask AnimateEvade(UnitFacade facade, int direction)
		{
			var originalPosition = facade.BodyRenderer.transform.localPosition;

			await DOTween.Sequence()
				.Append(facade.BodyRenderer.transform.DOLocalMoveX(originalPosition.x + -0.3f * direction, 0.2f))
				.Append(facade.BodyRenderer.transform.DOLocalMoveX(originalPosition.x, 0.1f))
			;
		}

		public static async UniTask AnimateHit(UnitFacade facade, int direction)
		{
			await DOTween.Sequence()
				.Append(facade.BodyPivot.transform.DORotate(new Vector3(0f, 0f, direction > 0 ? 10f : -10f), 0.1f))
				.Join(facade.BodyPivot.transform.DOScaleY(1.1f, 0.1f))
				.Append(facade.BodyPivot.transform.DORotate(new Vector3(0f, 0f, 0f), 0.1f))
				.Join(facade.BodyPivot.transform.DOScaleY(1f, 0.1f))
			;
		}

		public static void ShowInfos(Unit unit)
		{
			unit.Facade.InfosNameText.text = unit.Name;
			unit.Facade.InfosHealthText.text = $"{unit.HealthCurrent} / {unit.HealthMax}";
			var scale = unit.Facade.InfosHealthImage.transform.localScale;
			scale.x = (float) unit.HealthCurrent / unit.HealthMax;
			unit.Facade.InfosHealthImage.transform.localScale = scale;
			unit.Facade.InfosBodyImage.transform.rotation = unit.Facade.BodyRenderer.transform.rotation;
			unit.Facade.InfosCanvas.gameObject.SetActive(true);
		}

		public static void HideInfos(Unit unit)
		{
			unit.Facade.InfosCanvas.gameObject.SetActive(false);
		}

		private static void SetName(UnitFacade facade, string value)
		{
			facade.name = $"{value}";
		}

		private static async UniTask MoveTo(UnitFacade facade, Vector3Int destination, bool reverseHands = true)
		{
			const float durationPerUnit = 0.15f;

			var direction = destination.x - facade.transform.position.x;
			if (NeedsToRotate(facade, (int) direction))
			{
				await AnimateChangeDirection(facade, direction > 0f ? Unit.Directions.Right : Unit.Directions.Left);
			}

			var distance = Vector3.Distance(facade.transform.position, destination);
			var animDuration = distance * durationPerUnit;
			var handsOffset = reverseHands ? 1f : -1f;

			await DOTween.Sequence()
					.Append(facade.transform.DOMove(destination, animDuration).SetEase(Ease.Linear))
					.Join(
						DOTween.Sequence()
							.Append(facade.LeftHandRenderer.transform.DOLocalMove(facade.LeftHandRenderer.transform.localPosition + new Vector3(+0.1f, +0.1f, 0f) * handsOffset, animDuration / 2))
							.Join(facade.RightHandRenderer.transform.DOLocalMove(facade.RightHandRenderer.transform.localPosition + new Vector3(-0.1f, -0.1f, 0f) * handsOffset, animDuration / 2))
							.Append(facade.LeftHandRenderer.transform.DOLocalMove(facade.LeftHandRenderer.transform.localPosition, animDuration / 2))
							.Join(facade.RightHandRenderer.transform.DOLocalMove(facade.RightHandRenderer.transform.localPosition, animDuration / 2))
					)
					.Join(DOTween.Sequence()
						.Append(facade.BodyPivot.transform.DOScale(new Vector3(0.9f, 1.1f, 1f), animDuration / 2))
						.Append(facade.BodyPivot.transform.DOScale(new Vector3(1f, 1f, 1f), animDuration / 2))
					)
				;
		}

		private static bool NeedsToRotate(UnitFacade facade, int direction)
		{
			return direction != facade.BodyRenderer.transform.right.x && Mathf.Abs(direction) != 0;
		}
	}
}
