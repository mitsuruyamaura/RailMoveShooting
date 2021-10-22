using UniRx;

public enum ARState
{
    Debug,         // Editor �ł̃f�o�b�O�p
    Tracking,     // ���ʊ��m��
    Wait,         // �ҋ@�B�ǂ��̃X�e�[�g�ɂ������Ȃ����
    Ready,        // �Q�[��������
    Play,         // �Q�[����
    GameUp,       // �Q�[���I��
}

[System.Serializable]
public class ARStateReactiveProperty : ReactiveProperty<ARState>
{
    // �R���X�g���N�^
    public ARStateReactiveProperty() { }

    public ARStateReactiveProperty(ARState arState) : base(arState) { }
}
