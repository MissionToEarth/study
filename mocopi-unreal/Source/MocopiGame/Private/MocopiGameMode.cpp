#include "MocopiGameMode.h"
#include "MocopiGameCharacter.h"

AMocopiGameMode::AMocopiGameMode()
{
	DefaultPawnClass = AMocopiGameCharacter::StaticClass();
}
