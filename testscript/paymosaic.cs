using System.Collections.Generic;
using Symnity.Http;
using Symnity.Infrastructure;
using Symnity.Model.Accounts;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using UnityEngine;

public class paymosaic : MonoBehaviour
{
    private TransactionRepository transactionRepository;
    private Account signerAccount;

    private void Start()
    {
        transactionRepository = new TransactionRepository(setconst.node);
    }

    public async void Pay(float amount, string mosaic)
    {
        //�����
        var address = Address.CreateFromRawAddress(setconst.signeraddress);

        //���茳�i���͂����A�����ɂ��Ă�����̔閧�������J�����ɃA�J�E���g������Ă�邩�j
        signerAccount = Account.CreateFromPrivateKey(Wallet.GSkey, NetworkType.TEST_NET);

        var mosaicList = new List<Mosaic>() { new Mosaic(new MosaicId(mosaic), long.Parse(amount.ToString())) };

        var epocAdjustment = await HttpUtilities.GetEpochAdjustment(setconst.node);
        var generationHash = await HttpUtilities.GetGenerationHash(setconst.node);

        var transferTransaction = TransferTransaction.Create(
            Deadline.Create(epocAdjustment),
            address,
            mosaicList,
            PlainMessage.Create(""),
            NetworkType.TEST_NET
        );
        var aggregateTx = AggregateTransaction.CreateComplete(
                Deadline.Create(epocAdjustment),
                new List<Transaction>
                {
                    transferTransaction.ToAggregate(signerAccount.GetPublicAccount()),

                },
                setconst._networkType,
                new List<AggregateTransactionCosignature>()
                ).SetMaxFeeForAggregate(100, 2);

        var signedTransaction = signerAccount.SignTransactionWithCosignatories(
            aggregateTx,
            new List<Account> { signerAccount },
            generationHash);
        var result = await transactionRepository.Announce(signedTransaction);
        //Debug.Log(result);
    }
}
