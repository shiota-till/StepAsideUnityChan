using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
    //�A�j���[�V�������邽�߂̃R���|�[�l���g������
    private Animator myAnimator;

    //Unity�������ړ�������R���|�[�l���g������
    private Rigidbody myRigidbody;

    //�O�����̑��x
    private float velocityZ = 16f;

    //�������̑��x
    private float velocityX = 10f;

    //������̑��x
    private float velocityY = 10f;

    //���E�̈ړ��ł���͈�
    private float movebleRange = 3.4f;

    //����������������W��
    private float coefficient = 0.99f;

    //�Q�[���I���̔���
    private bool isEnd = false;

    //�Q�[���I�����ɕ\������e�L�X�g
    private GameObject stateText;

    //�X�R�A��\������e�L�X�g
    private GameObject scoreText;

    //���_
    private int score = 0;

    //���{�^�������̔���
    private bool isLButtonDown = false;

    //�E�{�^�������̔���
    private bool isRButtonDown = false;

    //Jump�{�^�������̔���
    private bool isJButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        //Animator�R���|�[�l���g���擾
        this.myAnimator = GetComponent<Animator>();

        //����A�j���[�V�������J�n
        this.myAnimator.SetFloat("Speed", 1);

        //Rigidbody�R���|�[�l���g���擾
        this.myRigidbody = GetComponent<Rigidbody>();

        //�V�[������stateText�I�u�W�F�N�g���擾
        this.stateText = GameObject.Find("GameResultText");

        //�V�[������stateText�I�u�W�F�N�g���擾
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[���I���Ȃ�Unity�����̓�������������
        if (this.isEnd)
        {
            this.velocityX *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.velocityZ *= this.coefficient;
            this.myAnimator.speed *= coefficient;
        }

        //�������̓��͂ɂ�鑬�x
        float inputVelocityX = 0;

        //������̓��͂ɂ�鑬�x
        float inputVelocityY = 0;

        //Unity��������L�[�܂��̓{�^���ɉ����č��E�Ɉړ�������
        if ((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown ) && -this.movebleRange < this.transform.position.x)
        {
            inputVelocityX = -this.velocityX;
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown ) && this.movebleRange > this.transform.position.x)
        {
            inputVelocityX = this.velocityX;
        }

        //�W�����v���Ă��Ȃ����ɃX�y�[�X�������ꂽ��W�����v����
        if ((Input.GetKeyDown(KeyCode.Space) || this.isJButtonDown ) && this.transform.position.y < 0.5f)
        {
            //�W�����v�A�j�����Đ�
            this.myAnimator.SetBool("Jump", true);

            //������ւ̑��x����
            inputVelocityY = this.velocityY;
        }
        else
        {
            //���݂�Y���̑��x����
            inputVelocityY = this.myRigidbody.velocity.y;
        }

        //Jump�X�e�[�g�̏ꍇ��Jump��false���Z�b�g����
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }

        this.myRigidbody.velocity = new Vector3(inputVelocityX, inputVelocityY, this.velocityZ);
    }
    private void OnTriggerEnter(Collider other) //�g���K�[���[�h�ő��̃I�u�W�F�N�g�ƐڐG�����ꍇ�̏���
    {
        //��Q���ɏՓ˂����ꍇ
        if (other.gameObject.tag =="CarTag"
            ||
            other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            //stateText��GAME OVER��\��
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }

        //�S�[���n�_�ɓ��B�����ꍇ
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            //stateText��CLEAR��\��
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }

        //�R�C���ɏՓ˂����ꍇ
        if (other.gameObject.tag == "CoinTag")
        {
            //���_�����Z
            this.score += 10;

            //ScoreText�Ɋl�������_����\��
            this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";

            //�p�[�e�B�N�����Đ�
            GetComponent<ParticleSystem>().Play();

            //�ڐG�����R�C���̃I�u�W�F�N�g��j��
            Destroy(other.gameObject);
        }
    }
    public void GetMyJumpButtonDown()�@//�W�����v�{�^�����������ꍇ�̏���
    {
        this.isJButtonDown = true;
    }
    public void GetMyJumpButtonUp()�@//�W�����v�{�^���𗣂����ꍇ�̏���
    {
        this.isJButtonDown = false;
    }
    public void GetMyLeftButtonDown()�@//���{�^���������������ꍇ�̏���
    {
        this.isLButtonDown = true;
    }
    public void GetMyLeftButtonUp()�@//���{�^���𗣂����ꍇ�̏���
    {
        this.isLButtonDown = false;
    }
    public void GetMyRightButtonDown()�@//�E�{�^���������������ꍇ�̏���
    {
        this.isRButtonDown = true;
    }
    public void GetMyRightButtonUp()�@//�E�{�^���𗣂����ꍇ�̏���
    {
        this.isRButtonDown = false;
    }
}
