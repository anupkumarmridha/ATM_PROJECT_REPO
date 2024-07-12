const depositAmount = 20000;
const withdrawAmount = 10000;

var accountNumber;
var currentBalance;

const baseUrl = "https://localhost:7151/api/ATMService";

const validateCard = async (cardNumber) => {
  try {
    const data = await fetch(`${baseUrl}/validate-card`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(cardNumber),
    })
      .then((response) => response.json())
      .then((response) => {
        accountNumber = response.accountNumber;
        currentBalance = response.currentBalance;
        return true;
      })
      .catch((error) => {
        console.error(error);
        return false;
      });

    if (data) {
      return true;
    }
  } catch (error) {
    console.error(error);
  }
};

const validatePin = async (cardNumber, cardPin) => {
  try {
    if (!cardNumber && !cardPin && cardPin.length === 4) {
      return false;
    }
    const data = await fetch(`url`)
      .then((response) => response.json())
      .catch((error) => console.error(error));

    if (data) {
      return true;
    }
  } catch (error) {
    console.error(error);
  }
};

const deposit = async (Amount) => {
  try {
    const data = await fetch(`${baseUrl}/Deposite`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        accountNumber: accountNumber,
        amount: Amount,
      }),
    })
      .then((response) => response.json())
      .then((response) => {
        return response;
      })
      .catch((error) => console.error(error));

    if (data) {
      return data;
    }
  } catch (error) {
    console.error(error);
  }
};

const withdraw = async (Amount) => {
  try {
    const data = await fetch(`${baseUrl}/Withdraw`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        accountNumber: accountNumber,
        amount: Amount,
      }),
    })
      .then((response) => response.json())
      .then((response) => {
        return response;
      })
      .catch((error) => console.error(error));

    if (data) {
      return data;
    }
  } catch (error) {
    console.error(error);
  }
};

const checkBalance = async () => {
  try {
    const data = await fetch(`${baseUrl}/?accountNo=${accountNumber}`)
      .then((response) =>
        response.json().then((response) => {
          return response;
        })
      )
      .catch((error) => console.error(error));

    if (data) {
      return data;
    }
  } catch (error) {
    console.error(error);
  }
};

async function checkCardNumber() {
  const cardNum = document.getElementById("card-number").value;
  if (cardNum.length != 16) {
    showError(
      "Please enter valid card number",
      "card-number",
      "card-number-error-container"
    );
  } else {
    makeErrorNone("card-number", "card-number-error-container");
  }

  const result = await validateCard(cardNum);
  if (result) {
    makeAllContainerDisplayNone();
    document.getElementById("services-container").style.display = "flex";
  }
}

async function showBalance() {
  const balance = await checkBalance();

  if (balance) {
    document.getElementById("balance").innerHTML = balance;
    makeAllContainerDisplayNone();
    document.getElementById("balance-container").style.display = "flex";
  }
}

async function showDeposit() {
  makeAllContainerDisplayNone();
  document.getElementById("deposit-container").style.display = "flex";
}

const validateDeposite = async () => {
  const amount = document.getElementById("deposit-value").value;
  console.log(amount);
  if (amount % 100 != 0) {
    showError(
      "Please enter amount in multiple of 100",
      "deposit-value",
      "deposit-error-container"
    );
  } else {
    makeErrorNone("deposit-value", "deposit-error-container");
    const result = await deposit(amount);
    console.log(result);
    if (result) {
      makeAllContainerDisplayNone();
      document.getElementById("deposit-container").style.display = "flex";
    }
  }
};

async function showWithdraw() {
  makeAllContainerDisplayNone();
  document.getElementById("withdraw-container").style.display = "flex";
}

async function validateWithdraw() {
  const amount = document.getElementById("withdraw-amount").value;
  if (amount % 100 != 0) {
    showError(
      "Please enter amount in multiple of 100",
      "withdraw-amount",
      "withdraw-error-container"
    );
  } else {
    makeErrorNone("withdraw-amount", "withdraw-error-container");
    const result = await withdraw(amount);
    if (result) {
      makeAllContainerDisplayNone();
      document.getElementById("withdraw-container").style.display = "flex";
    }
  }
}

function showError(message, inputBoxId, errorContainerId) {
  const inputBox = document.getElementById(inputBoxId);
  const errorContainer = document.getElementById(errorContainerId);
  inputBox.style.borderColor = "Red";
  inputBox.style.outlineColor = "Red";
  errorContainer.innerHTML = message;
}

function makeErrorNone(inputBoxId, errorContainerId) {
  const inputBox = document.getElementById(inputBoxId);
  const errorContainer = document.getElementById(errorContainerId);
  inputBox.style.borderColor = "black";
  inputBox.style.outlineColor = "black";
  errorContainer.style.display = "none";
}

function makeAllContainerDisplayNone() {
  document.getElementById("card-container").style.display = "none";
  document.getElementById("pin-container").style.display = "none";
  document.getElementById("deposit-container").style.display = "none";
  document.getElementById("withdraw-container").style.display = "none";
  document.getElementById("balance-container").style.display = "none";
  document.getElementById("otp-container").style.display = "none";
  document.getElementById("services-container").style.display = "none";
}

const backToServicePage = () => {
  makeAllContainerDisplayNone();
  document.getElementById("services-container").style.display = "flex";
};
