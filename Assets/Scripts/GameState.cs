/// <summary>
/// �Q�[���̐i�s��Ԃ̎��
/// </summary>
public enum GameState {
    Debug,           // AR ���ɃG�f�B�^�[�Ńf�o�b�O���s���ۂɓK�p
    Tracking,        // AR �̃g���b�L���O��
    Wait,            // �g���b�L���O������A�Q�[���̏���
    Play_Move,       // �ړ���
    Play_Mission,    // �~�b�V������
    GameUp,          // �Q�[���I��(�Q�[���I�[�o�[�A�Q�[���N���A)
}