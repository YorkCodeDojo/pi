import Foundation

let fullPi = FullPi()

let highPrecisionHandler = NSDecimalNumberHandler(
    roundingMode: .plain,
    scale: 100,
    raiseOnExactness: false,
    raiseOnOverflow: false,
    raiseOnUnderflow: false,
    raiseOnDivideByZero: true
)

let twentyTwo = NSDecimalNumber(decimal: Decimal(22))
let seven = NSDecimalNumber(decimal: Decimal(7))
var roughPi = twentyTwo.dividing(by: seven, withBehavior: highPrecisionHandler).decimalValue

var swiftPi = Double.pi

let swiftPiAccuracy = compareDecimalAccuracy(approximation: Decimal(swiftPi), reference: fullPi.asString)
print("Swift Foundation π is accurate to \(swiftPiAccuracy) decimal places (approximation: \(swiftPi))")


/*  22/7 */
let roughPiAccuracy = compareDecimalAccuracy(approximation: roughPi, reference: fullPi.asString)
print("22/7 is accurate to \(roughPiAccuracy) decimal places (approximation: \(roughPi))")

/*  Leibniz */
let leibnizIterations = Int(pow(10.0, 4))
let leibnizPi = calculateLeibnizPi(iterations: leibnizIterations)

let leibnizPiAccuracy = compareDecimalAccuracy(approximation: leibnizPi, reference: fullPi.asString)
print("Leibniz π is accurate to \(leibnizPiAccuracy) decimal places (using \(leibnizIterations) iterations) (approximation: \(leibnizPi))")

func calculateLeibnizPi(iterations: Int, handler: NSDecimalNumberHandler = highPrecisionHandler) -> Decimal {
    var sum: Decimal = 0
    
    for k in 0..<iterations {
        let denominator = Decimal(2 * k + 1)
        
        let one = NSDecimalNumber(decimal: Decimal(1))
        let denom = NSDecimalNumber(decimal: denominator)
        let term = one.dividing(by: denom, withBehavior: handler).decimalValue
        
        sum += pow(-1,k) * term
    }

    return sum * 4
}

/*  Nilakantha */
let nilakanthaIterations = Int(pow(10.0, 4))
let nilakanthaPi = calculateNilakanthaPi(iterations: nilakanthaIterations)

let nilakanthaPiAccuracy = compareDecimalAccuracy(approximation: nilakanthaPi, reference: fullPi.asString)
print("Nilakantha π is accurate to \(nilakanthaPiAccuracy) decimal places (using \(nilakanthaIterations) iterations) (approximation: \(nilakanthaPi))")

func calculateNilakanthaPi(iterations: Int, handler: NSDecimalNumberHandler = highPrecisionHandler) -> Decimal {
    var sum: Decimal = 3
    
    for k in 1...iterations {
        let n = 2 * k
        let denominator = Decimal(n * (n + 1) * (n + 2))
        
        let four = NSDecimalNumber(decimal: Decimal(4))
        let denom = NSDecimalNumber(decimal: denominator)
        let term = four.dividing(by: denom, withBehavior: handler).decimalValue
        
        sum += pow(-1,k+1) * term
    }

    return sum
}

func compareDecimalAccuracy(approximation: Decimal, reference: String) -> Int {
    
    let formatter = NumberFormatter()
    formatter.numberStyle = .decimal
    formatter.maximumFractionDigits = 100000000
    formatter.usesGroupingSeparator = false
    
    guard let approximationFormatted = formatter.string(from: NSDecimalNumber(decimal: approximation)),
          let approximationDecimalIndex = approximationFormatted.firstIndex(of: "."),
          let referenceDecimalIndex = reference.firstIndex(of: ".") else {
        return 0
    }
    
    let approximationDecimals = String(approximationFormatted[approximationFormatted.index(after: approximationDecimalIndex)...])
    let referenceDecimals = String(reference[reference.index(after: referenceDecimalIndex)...])
    
    var matchingDigits = 0
    let minLength = min(approximationDecimals.count, referenceDecimals.count)
    
    for i in 0..<minLength {
        let approximationDigit = approximationDecimals[approximationDecimals.index(approximationDecimals.startIndex, offsetBy: i)]
        let referenceDigit = referenceDecimals[referenceDecimals.index(referenceDecimals.startIndex, offsetBy: i)]
        
        if approximationDigit == referenceDigit {
            matchingDigits += 1
        } else {
            break // Stop at first mismatch
        }
    }
    
    return matchingDigits
}
