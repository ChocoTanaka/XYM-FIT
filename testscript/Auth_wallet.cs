using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using Symnity.Core.Crypto;
using Symnity.Http;
using Symnity.Model.Accounts;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using Symnity.Infrastructure;

public class Auth_wallet : MonoBehaviour
{
    public Text check;
    private TransactionRepository transactionRepository;

    public void checkstring()
    {
        transactionRepository = new TransactionRepository(setconst.node);
        StartCoroutine("Authcheck");
    }

    IEnumerator Authcheck()
    {
        string add1 = Wallet.GSwalletaddress[0].ToString();
        string add2 = Wallet.GSwalletaddress[1].ToString();
        string add39 = Wallet.GSwalletaddress[38].ToString();

        //1�����ڂ̃`�F�b�N
        bool check1 = Regex.IsMatch(add1, @"[N,T]");
        //2�����ڂ̃`�F�b�N
        bool check2 = Regex.IsMatch(add2, @"[A,B,C,D]");
        //���[�̃`�F�b�N
        bool check39 = Regex.IsMatch(add39, @"[A,I,Q,Y]");
        //���͌��󂱂������d�g�݂ł��邱�Ƃ����킩��Ȃ�����
        bool checkkey = Regex.IsMatch(Wallet.GSkey, @"[A-F0-9]");

        if (Wallet.GSwalletaddress.Length == 39 && Wallet.GSkey.Length == 64 && check1 && check2 && check39 && checkkey)
        {
            //�F�؂����������Ƃ�
            check.gameObject.SetActive(true);
            check.text = "Connected!";
            //�}���`�V�O�o�^
            multisig();
            //playerprefs�ɃZ�[�u
            //PlayerPrefs.SetString("Walletaddress", Wallet.GSwalletaddress);
            //�Z�L�����e�B�̓s����Í������Ă�������
            //var encryptedPrivateKey = Crypto.EncryptString(Wallet.GSkey, Wallet.GSwalletaddress);
            //PlayerPrefs.SetString("key", encryptedPrivateKey);
            yield return new WaitForSeconds(2);
            check.gameObject.SetActive(false);
            SceneManager.LoadScene("home");
        }
        else
        {
            check.gameObject.SetActive(true);
            check.text = "Wrong column";
            yield return new WaitForSeconds(2);
            check.gameObject.SetActive(false);
        }
    }

    private async void multisig()
    {
        //���ɑ�
        Account bob = Account.CreateFromPrivateKey(setconst.signerkey, setconst._networkType);
        //�A�J�E���g��
        Account carol = Account.CreateFromPrivateKey(Wallet.GSkey, setconst._networkType);

        var epocAdjustment = await HttpUtilities.GetEpochAdjustment(setconst.node);
        var generationHash = await HttpUtilities.GetGenerationHash(setconst.node);

        var multisigTx = MultisigAccountModificationTransaction.Create(
            Deadline.Create(epocAdjustment),
            1, //minApproval:���F�̂��߂ɕK�v�ȍŏ������Ґ�����
            1, //minRemoval:�����̂��߂ɕK�v�ȍŏ������Ґ�����
            new List<UnresolvedAddress>()
            {
                carol.Address
            },//�ǉ��ΏۃA�h���X���X�g
            new List<UnresolvedAddress>(),//�����ΏۃA�h���X���X�g
            setconst._networkType
            );

        var aggregateTx = AggregateTransaction.CreateComplete(
                Deadline.Create(epocAdjustment),
                new List<Transaction>
                {
                    multisigTx.ToAggregate(bob.GetPublicAccount()),

                },
                setconst._networkType,
                new List<AggregateTransactionCosignature>()
                ).SetMaxFeeForAggregate(100, 2);

        var signedTx = aggregateTx.SignTransactionWithCosignatories(
                bob,
                new List<Account> { carol },
                generationHash
            );

        string result = await transactionRepository.Announce(signedTx);
        Debug.Log(result);
    }
}
