#include <algorithm>
#include <print>
#include <random>
#include <ranges>

auto count_pi_digits(double val) -> std::size_t {
  using namespace std::string_view_literals;
  constexpr auto pi_whole_part = "3"sv;
  constexpr auto pi_fraction_part =
      "14159265358979323846264338327950288419716939937510582097494459230781640628"sv;

  char buffer[80];
  auto [ptr, ec] =
      std::to_chars(std::begin(buffer), std::end(buffer), val,
                    std::chars_format::fixed, pi_fraction_part.size());
  if (ec != std::errc{}) {
    throw std::runtime_error("std::to_chars conversion failed");
  }
  auto const s = std::string_view(buffer, ptr);

  auto const dot = std::ranges::find(s, '.');
  auto const whole_part = std::ranges::subrange{s.begin(), dot};
  // The whole part must be "3"
  if (!std::ranges::equal(whole_part, pi_whole_part)) {
    return 0;
  }
  auto const fraction_part = std::ranges::subrange{std::next(dot), s.end()};
  auto const [it, _] = std::ranges::mismatch(fraction_part, pi_fraction_part);
  return std::distance(fraction_part.begin(), it) + 1;
}

void ex1() {
  auto const a = 22.0;
  auto const b = 7.0;
  auto const simple_pi = a / b;
  std::println("{:.50} == {}/{} matches {} digits", simple_pi, a, b,
               count_pi_digits(simple_pi));
}

void ex2() {
  auto best_a = 0.0;
  auto best_b = 0.0;
  auto best_digits = 0;

  constexpr double pi_lower = 3.1;
  constexpr double pi_upper = 3.2;

  for (int a : std::views::iota(1, 10'000)) {
    int const b_min = std::max(1, static_cast<int>(a / pi_upper));
    int const b_max = static_cast<int>(a / pi_lower) + 1;
    for (int b : std::views::iota(b_min, b_max)) {
      auto const val = static_cast<double>(a) / b;
      auto const digits = count_pi_digits(val);
      if (digits > best_digits) {
        std::tie(best_a, best_b, best_digits) = std::tie(a, b, digits);
      }
    }
  }
  std::println("{:.50} == {}/{} matches {} digits", best_a / best_b, best_a,
               best_b, best_digits);
}

auto gregory_leibniz(int n) -> std::pair<double, int> {
  auto acc = 0.0;
  for (auto k : std::views::iota(0)) {
    acc += ((k % 2 == 0) ? 1.0 : -1.0) * (4.0 / (2 * k + 1));
    if (count_pi_digits(acc) >= n)
      return {acc, k + 1};
  }
  std::unreachable();
}

void ex3() {
  auto const [val1, iter1] = gregory_leibniz(6);
  std::println(
      "{:.50} Gregory-Leibniz series matched {} digits in {} iterations", val1,
      6, iter1);
}

auto nilakantha(int n) -> std::pair<double, int> {
  auto acc = 3.0;
  for (auto k : std::views::iota(1)) {
    auto const a = 2 * k;
    acc += ((k % 2 != 0) ? 1.0 : -1.0) * (4.0 / (a * (a + 1) * (a + 2)));
    if (count_pi_digits(acc) >= n)
      return {acc, k};
  }
  std::unreachable();
}

void ex4() {
  auto const [val2, iter2] = nilakantha(6);
  std::println("{:.50} Nilakantha series matched {} digits in {} iterations",
               val2, 6, iter2);
}

auto dart_board_method(std::size_t total) -> double {
  auto rng = std::mt19937{std::random_device{}()};

  auto const count =
      std::ranges::count_if(std::views::iota(0uz, total), [&](auto) {
        auto dist = std::uniform_real_distribution(-1.0, 1.0);
        return std::hypot(dist(rng), dist(rng)) <= 1.0;
      });

  return (4.0 * count) / total;
}

void ex5() {
  constexpr auto iterations = 10'000'000uz;
  auto const pi = dart_board_method(iterations);
  auto const digits = count_pi_digits(pi);
  std::println("{:.50} dart board method matched {} digits in {} iterations",
               pi, digits, iterations);
}

int main() {
  ex1();
  ex2();
  ex3();
  ex4();
  ex5();
}
