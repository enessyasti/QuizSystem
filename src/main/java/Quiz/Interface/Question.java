package Quiz.Interface;

import java.util.List;

public interface Question {
    String getPrompt();                   // Soru metni
    List<? extends Answer> getAnswers();  // Cevap seçenekleri (sadece okunur)
    int getCorrectAnswerIndex();          // Doğru cevabın sırası (0,1,2...)
}