using UnityEngine;

/// <summary>
/// �G�l�~�[�̕��ʂ��Ƃ̃_���[�W���v�Z����N���X
/// </summary>
public class BodyRegionPartsController : MonoBehaviour
{
    [SerializeField, Header("���ʂ̐ݒ�")]
    private BodyRegionType bodyPartType;

    //private EnemyBase enemyController;


    //public void SetUpPartsController(EnemyBase enemyController) {
    //    this.enemyController = enemyController;
    //}

    /// <summary>
    /// ���ʂ̏��̎擾�p�B�v���p�e�B�ł���
    /// </summary>
    /// <returns></returns>
    public BodyRegionType GetBodyPartType() {
        return bodyPartType;
    }

    /// <summary>
    /// ���ʂ��ƂɃ_���[�W�̒l���v�Z
    /// </summary>
    /// <param name="damage"></param>
    public (int, BodyRegionType) CalcDamageParts(int damage) {

        int lastDamage = bodyPartType switch {
            BodyRegionType.Head => damage * 5,

            // TODO ���̕��ʂ�ǉ�

            _ => damage * 1,
        };

        return (lastDamage, bodyPartType); 

        //enemyController.CalcDamage(lastDamage, bodyPartType);
    }
}
