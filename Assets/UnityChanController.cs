using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;

    //Unityちゃんを移動させるコンポーネントを入れる
    private Rigidbody myRigidbody;

    //前方向の速度
    private float velocityZ = 16f;

    //横方向の速度
    private float velocityX = 10f;

    //上方向の速度
    private float velocityY = 10f;

    //左右の移動できる範囲
    private float movebleRange = 3.4f;

    //動きを減速させる係数
    private float coefficient = 0.99f;

    //ゲーム終了の判定
    private bool isEnd = false;

    //ゲーム終了時に表示するテキスト
    private GameObject stateText;

    //スコアを表示するテキスト
    private GameObject scoreText;

    //得点
    private int score = 0;

    //左ボタン押下の判定
    private bool isLButtonDown = false;

    //右ボタン押下の判定
    private bool isRButtonDown = false;

    //Jumpボタン押下の判定
    private bool isJButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();

        //走るアニメーションを開始
        this.myAnimator.SetFloat("Speed", 1);

        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();

        //シーン中のstateTextオブジェクトを取得
        this.stateText = GameObject.Find("GameResultText");

        //シーン中のstateTextオブジェクトを取得
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了ならUnityちゃんの動きを減衰する
        if (this.isEnd)
        {
            this.velocityX *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.velocityZ *= this.coefficient;
            this.myAnimator.speed *= coefficient;
        }

        //横方向の入力による速度
        float inputVelocityX = 0;

        //上方向の入力による速度
        float inputVelocityY = 0;

        //Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
        if ((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown ) && -this.movebleRange < this.transform.position.x)
        {
            inputVelocityX = -this.velocityX;
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown ) && this.movebleRange > this.transform.position.x)
        {
            inputVelocityX = this.velocityX;
        }

        //ジャンプしていない時にスペースが押されたらジャンプする
        if ((Input.GetKeyDown(KeyCode.Space) || this.isJButtonDown ) && this.transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生
            this.myAnimator.SetBool("Jump", true);

            //上方向への速度を代入
            inputVelocityY = this.velocityY;
        }
        else
        {
            //現在のY軸の速度を代入
            inputVelocityY = this.myRigidbody.velocity.y;
        }

        //Jumpステートの場合はJumpにfalseをセットする
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }

        this.myRigidbody.velocity = new Vector3(inputVelocityX, inputVelocityY, this.velocityZ);
    }
    private void OnTriggerEnter(Collider other) //トリガーモードで他のオブジェクトと接触した場合の処理
    {
        //障害物に衝突した場合
        if (other.gameObject.tag =="CarTag"
            ||
            other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            //stateTextにGAME OVERを表示
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }

        //ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            //stateTextにCLEARを表示
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }

        //コインに衝突した場合
        if (other.gameObject.tag == "CoinTag")
        {
            //得点を加算
            this.score += 10;

            //ScoreTextに獲得した点数を表示
            this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";

            //パーティクルを再生
            GetComponent<ParticleSystem>().Play();

            //接触したコインのオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }
    public void GetMyJumpButtonDown()　//ジャンプボタンを押した場合の処理
    {
        this.isJButtonDown = true;
    }
    public void GetMyJumpButtonUp()　//ジャンプボタンを離した場合の処理
    {
        this.isJButtonDown = false;
    }
    public void GetMyLeftButtonDown()　//左ボタンを押し続けた場合の処理
    {
        this.isLButtonDown = true;
    }
    public void GetMyLeftButtonUp()　//左ボタンを離した場合の処理
    {
        this.isLButtonDown = false;
    }
    public void GetMyRightButtonDown()　//右ボタンを押し続けた場合の処理
    {
        this.isRButtonDown = true;
    }
    public void GetMyRightButtonUp()　//右ボタンを離した場合の処理
    {
        this.isRButtonDown = false;
    }
}
